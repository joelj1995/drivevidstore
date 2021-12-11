using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriveVidStore_Api.Integrations
{
    public sealed class FireBaseIntegration
    {
        private static readonly FirebaseApp _app = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.GetApplicationDefault()
        });

        public FirebaseApp app
        {
            get => _app;
        }

        public FirebaseAuth auth
        {
            get => FirebaseAuth.GetAuth(app);
        }

        public async Task<bool> CreateAccount(string email, string password)
        {
            await auth.CreateUserAsync(new UserRecordArgs
            {
                Email = email,
                Password = password
            });
            return true;
        }
    }
}
