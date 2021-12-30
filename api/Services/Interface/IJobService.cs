using DriveVidStore_Api.Models.Job;
using DrivVidStore_Common.Model;
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
