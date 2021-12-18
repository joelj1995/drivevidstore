using DriveVidStore_Api.Models.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage
using System.Text.Json;

namespace DriveVidStore_Api.Integrations
{
    public class AzureIntegration
    {
        private const string UPLOAD_JOBS_QUEUE_NAME = "uploadjobs";

        private string StorageAccountConnectionString => Environment.GetEnvironmentVariable("StorageAccountConnectionString");

        public async Task EnqueueJob(JobData jobData)
        {
            QueueClient queueClient = new QueueClient(StorageAccountConnectionString, UPLOAD_JOBS_QUEUE_NAME);
            await queueClient.CreateIfNotExistsAsync();

            if (queueClient.Exists())
            {
                var jobDataString = JsonSerializer.Serialize(jobData);
                queueClient.SendMessage(jobDataString);
            }
            else
            {
                throw new Exception("Failed to create queue");
            }
        }
    }
}
