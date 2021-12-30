using DrivVidStore_Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Interface
{
    public interface IProfileService
    {
        Task AddDriveApiKey(string user, DriveKey addKeyRequest);
        Task RemoveDriveApiKey(string user, DriveKey addKeyRequest);
        Task<IEnumerable<DriveKey>> GetKeys(string user);
    }
}
