using System;
using System.Collections.Generic;
using System.Text;

namespace DriveVidStore_Worker
{
    abstract class JobProcessingException : Exception
    {
        public virtual string JobErrorMessage() => "";
    }

    class AuthTokenExpiredException : JobProcessingException
    {
        public override string JobErrorMessage() => "Google Authentication token expired.";
    }

    class TimeoutException : JobProcessingException
    {
        public override string JobErrorMessage() => "The command did not complete within the timeout period.";
    }
}
