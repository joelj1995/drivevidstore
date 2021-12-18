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

                    var jobDataPath = DownloadJobAndReturnPath(jobUserId, jobId);
                    UploadJobDataToDrive(jobDataPath, jobApiKey);

                    // Delete the message
                    queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                }
            }
        }

        public static string DownloadJobAndReturnPath(string userId, string identifier)
        {
            var storageClient = StorageClient.Create();
            var destinationPath = @"c:\tmp-download\" + $"{userId}-{identifier}";
            
            using (FileStream fs = File.Create(destinationPath))
            {
                storageClient.DownloadObject("drivevidstore.appspot.com", $"{userId}/{identifier}", fs); // TODO: inject bucket name
            }
            return destinationPath;
        }

        public static void UploadJobDataToDrive(string jobDataPath, string jobApiKey)
        {
            throw new NotImplementedException();
        }
    }
}
