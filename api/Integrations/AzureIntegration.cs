using DriveVidStore_Api.Models.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Integrations
{
    public class AzureIntegration
    {
        private const string UPLOAD_JOBS_QUEUE_NAME = "UPLOAD_JOBS";

        public Task EnqueueJob(JobData jobData)
        {
            throw new NotImplementedException();
        }
    }
}
