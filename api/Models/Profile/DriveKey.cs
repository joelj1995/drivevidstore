using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Models.Profile
{
    public class DriveKey
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Key { get; set; }
    }
}
