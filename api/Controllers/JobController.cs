using DriveVidStore_Api.Models.Job;
using DriveVidStore_Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        [Route("{identifier}")]
        public async Task<IActionResult> EnqueueUpload([FromRoute] string identifier, EnqueueJobRequest jobRequest)
        {
            var userId = User.Claims.First(c => c.Type == "user_id").Value;
            var jobData = new JobData
            {
                User = userId,
                Identifier = identifier,
                ApiKey = jobRequest.ApiKey
            };
            await _jobService.EnqueueUpload(jobData);
            return Ok();
        }
    }
}
