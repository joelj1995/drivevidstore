using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Models.Auth
{
    public class CreateAccountResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
    }
}
