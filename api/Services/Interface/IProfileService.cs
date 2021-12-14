using DriveVidStore_Api.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Interface
{
    public interface IProfileService
    {
        Task AddDriveApiKey(string user, AddKeyRequest addKeyRequest);
    }
}
