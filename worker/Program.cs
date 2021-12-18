using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;

namespace DriveVidStore_Worker
{
    class Program
    {
        private const string UPLOAD_JOBS_QUEUE_NAME = "uploadjobs";
        private static string StorageAccountConnectionString => Environment.GetEnvironmentVariable("StorageAccountConnectionString");

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            while (true)
            {
                QueueClient queueClient = new QueueClient(StorageAccountConnectionString, UPLOAD_JOBS_QUEUE_NAME);

                if (queueClient.Exists())
                {
                    QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                    // Process (i.e. print) the message in less than 30 seconds
                    if (retrievedMessage.Length > 0)
                    {
                        Console.WriteLine($"Dequeued message: '{retrievedMessage[0].Body}'");

                        // Delete the message
                        queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    }
                }
                System.Threading.Thread.Sleep(50);
            }
            
        }
    }
}
