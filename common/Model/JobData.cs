using System;
using System.Collections.Generic;
using System.Text;

namespace DrivVidStore_Common.Model
{
    public class JobData
    {
        public string FileName { get; set; }
        public string User { get; set; }
        public string Identifier { get; set; }
        public string ApiKey { get; set; }
        public DateTime DateEnqueued { get => DateTime.Now; }
        // TODO: Add encoding attributes for the compression of the file
    }
}
