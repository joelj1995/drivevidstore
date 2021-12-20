using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Models.Job
{
    public class EnqueueJobRequest
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string FileName { get; set; }
    }
}
