using DriveVidStore_Api.Models.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Interface
{
    public interface IJobService
    {
        Task EnqueueUpload(JobData jobData);
    }
}
