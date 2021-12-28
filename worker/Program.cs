using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DriveVidStore_Worker
{
    class Program
    {
        private const string UPLOAD_JOBS_QUEUE_NAME = "uploadjobs";
        private static string StorageAccountConnectionString => Environment.GetEnvironmentVariable("StorageAccountConnectionString");
        private static string FfmpegPath => @"/3rd-party/ffmpeg-4.4.1-amd64-static/ffmpeg";
        private static TimeSpan AzureTimeout => new TimeSpan(0, 10, 0);

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
                QueueMessage[] retrievedMessage = queueClient.ReceiveMessages(visibilityTimeout: AzureTimeout);

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

                    try
                    {
                        var jobDataPath = DownloadJobAndReturnPath(jobUserId, jobId);
                        var processedJobDataPath = CompressJobDataAndReturnPath(jobDataPath);
                        UploadJobDataToDrive(processedJobDataPath, jobApiKey, jobFileName);
                        Console.WriteLine("Message processed successfully.");
                        queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    }
                    catch(JobProcessingException ex)
                    {
                        Console.WriteLine($"Message processing failed with known error {ex.JobErrorMessage()}");
                        queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    }

                    // TODO: Delete file from FireBase on success

                    // Delete the message
                    
                   
                }
            }
        }

        public static string DownloadJobAndReturnPath(string userId, string identifier)
        {
            var storageClient = StorageClient.Create();
            var destinationPath = @"/tmp/" + $"{userId}-{identifier}"; // TODO: Inject base path
            
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
            string compressVideoCommand = $"{FfmpegPath} -y -i {jobDatapath} -vcodec h264 -acodec mp3 {processedFilePath}";
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                startInfo.FileName = "/bin/bash";
                startInfo.Arguments = $"-c \"{command}\"";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $"/C {command}";
            }
            else
            {
                throw new Exception("Platform not supported");
            }
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo = startInfo;
            process.Start();
            if(!process.WaitForExit((AzureTimeout / 2).Milliseconds))
            {
                process.Kill();
                throw new TimeoutException();
            }
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
