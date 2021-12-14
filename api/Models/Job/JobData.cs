using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Models.Job
{
    public class JobData
    {
        public string User { get; set; }
        public string Identifier { get; set; }
        public string ApiKey { get; set; }
        // TODO: Add encoding attributes for the compression of the file
    }
}
