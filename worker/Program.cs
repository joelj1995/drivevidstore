using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DriveVidStore_Worker
{
    class Program
    {
        private const string UPLOAD_JOBS_QUEUE_NAME = "uploadjobs";
        private static string StorageAccountConnectionString => Environment.GetEnvironmentVariable("StorageAccountConnectionString");
        private static string FfmpegPath => @"C:\Users\colte\Downloads\ffmpeg-2021-12-23-git-60ead5cd68-essentials_build\bin\ffmpeg.exe"; // TODO: Get this from configuration

        static void Main(string[] args)
        {
            Console.WriteLine("Worker started!");
            while (true) // TODO: Add some type of kill switch to gracefully shut down
            {
                PollAndProcessJob();
                System.Threading.Thread.Sleep(50);
            }
        }

        public static void PollAndProcessJob()
        {
            QueueClient queueClient = new QueueClient(StorageAccountConnectionString, UPLOAD_JOBS_QUEUE_NAME);

            if (queueClient.Exists())
            {
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                // Process (i.e. print) the message in less than 30 seconds
                if (retrievedMessage.Length > 0)
                {
                    // TODO: Make sure timeout is aligned with the max time to process job
                    Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");
                    var messageBodyText = retrievedMessage[0].Body.ToString();
                    var messageBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageBodyText);

                    var jobUserId = messageBody["User"];
                    var jobId = messageBody["Identifier"];
                    var jobApiKey = messageBody["ApiKey"];
                    var jobFileName = messageBody["FileName"];

                    var jobDataPath = DownloadJobAndReturnPath(jobUserId, jobId);
                    var processedJobDataPath = CompressJobDataAndReturnPath(jobDataPath);
                    UploadJobDataToDrive(processedJobDataPath, jobApiKey, jobFileName);

                    // TODO: Delete file from FireBase on success

                    // Delete the message
                    queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    Console.WriteLine("Message processed successfully.");
                }
            }
        }

        public static string DownloadJobAndReturnPath(string userId, string identifier)
        {
            var storageClient = StorageClient.Create();
            var destinationPath = @"c:\tmp-download\" + $"{userId}-{identifier}"; // TODO: Inject destination path
            
            using (FileStream fs = File.Create(destinationPath))
            {
                storageClient.DownloadObject("drivevidstore.appspot.com", $"{userId}/{identifier}", fs); // TODO: inject bucket name
            }
            return destinationPath;
        }

        public static string CompressJobDataAndReturnPath(string jobDatapath)
        {
            Console.WriteLine(jobDatapath);
            string processedFilePath = jobDatapath + ".processed.mp4";
            string compressVideoCommand = $"{FfmpegPath} -i {jobDatapath} -vcodec h264 -acodec mp3 {processedFilePath}";
            Console.WriteLine(compressVideoCommand);
            var commandExitStatus = ExecuteCommandUnsafe(compressVideoCommand);
            if (commandExitStatus != 0)
            {
                // TODO: logic for processing failed status
                Console.WriteLine($"Failed to process video. Processed finished with result {commandExitStatus}");
            };
            return processedFilePath;
        }

        public static int ExecuteCommandUnsafe(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C {command}";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            return process.ExitCode;
        }

        public static void UploadJobDataToDrive(string processedJobDataPath, string jobApiKey, string fileName)
        {
            var client = new GoogleDriveResumableUploader(jobApiKey);
            using (FileStream fs = System.IO.File.OpenRead(processedJobDataPath))
            {
                client.UploadFile(fs, fileName);
            }
        }
    }
}
