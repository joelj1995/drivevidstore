using DriveVidStore_Api.Integrations;
using DriveVidStore_Api.Models.Job;
using DriveVidStore_Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Implementation
{
    public class JobService : IJobService
    {
        private readonly AzureIntegration _azureIntegration = new AzureIntegration();
        private readonly FireBaseIntegration _fb = new FireBaseIntegration();

        public async Task EnqueueUpload(JobData jobData)
        {
            // validate that the specified file identifier exists for the user
            // await _fb.ValidateJob(jobData);
            await _azureIntegration.EnqueueJob(jobData);
        }
    }
}
