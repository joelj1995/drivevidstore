using DrivVidStore_Common.Model;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivVidStore_Common.Integrations
{
    public sealed class FirebaseIntegration
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

        public FirestoreDb db
        {
            get => FirestoreDb.Create("drivevidstore");
        }

        public async Task CreateAccount(string email, string password)
        {
            await auth.CreateUserAsync(new UserRecordArgs
            {
                Email = email,
                Password = password
            });
        }

        public async Task AddApiKey(string userId, string name, string key)
        {
            dynamic[] keys = { new { name, key } };
            CollectionReference collection = db.Collection("users");
            DocumentReference document = collection.Document(userId);
            await document.SetAsync(new { DriveApiKeys = FieldValue.ArrayUnion(keys) }, SetOptions.MergeAll);
        }

        public async Task RemoveApiKey(string userId, string name, string key)
        {
            dynamic[] keys = { new { name, key } };
            CollectionReference collection = db.Collection("users");
            DocumentReference document = collection.Document(userId);
            await document.SetAsync(new { DriveApiKeys = FieldValue.ArrayRemove(keys) }, SetOptions.MergeAll);
        }

        public async Task<IEnumerable<DriveKey>> GetKeys(string userId)
        {
            var userReference = db.Collection("users").Document(userId);
            var keysReference = await userReference.GetSnapshotAsync();
            var keys = keysReference.GetValue<dynamic[]>("DriveApiKeys");
            return keys.Select(k => new DriveKey { Key = k["key"], Name = k["name"] });
        }

        public async Task ValidateJob(JobData jobData)
        {
            throw new NotImplementedException();
        }
    }
}
