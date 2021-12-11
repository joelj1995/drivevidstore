﻿using DriveVidStore_Api.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Services.Interface
{
    public interface IAuthService
    {
        LoginResponse Login(string userName, string password);
    }
}
