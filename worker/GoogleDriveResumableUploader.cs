using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DriveVidStore_Worker
{
    public class GoogleDriveResumableUploader
    {
        // Reference: https://developers.google.com/drive/api/v3/manage-uploads#http---multiple-requests

        const string BASE_URL = "https://www.googleapis.com/upload/drive/v3";

        const int ChunkSize = 256 * 1024 * 100;

        const int ChunkAttempts = 5;

        string StartUploadUrl { get => $"{BASE_URL}/files?uploadType=resumable"; }

        private readonly WebClient client;

        public GoogleDriveResumableUploader(string token)
        {
            client = new WebClient();
            client.Headers.Add("Authorization", $"Bearer {token}");
        }

        public void UploadFile(FileStream fileStream, string name)
        {
            var location = SendInitialRequest(name);
            UploadTheContent(fileStream, location);
        }

        public string SendInitialRequest(string name)
        {
            dynamic fileMetadata = new
            {
                name = name
            };
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            string fileMetadataJson = JsonConvert.SerializeObject(fileMetadata);
            client.UploadString(StartUploadUrl, fileMetadataJson);
            var responseHeaders = client.ResponseHeaders;
            return responseHeaders.Get("Location");
        }

        public void UploadTheContent(FileStream content, string location)
        {
            using (BufferedStream bs = new BufferedStream(content, ChunkSize))
            {
                byte[] buffer = new byte[ChunkSize];
                int byteRead;
                while ((byteRead = bs.Read(buffer, 0, ChunkSize)) > 0)
                {
                    byte[] originalBytes;
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        mStream.Write(buffer, 0, byteRead);
                        originalBytes = mStream.ToArray();
                        var start = bs.Position - byteRead;
                        var end = bs.Position - 1;
                        var total = content.Length;
                        for (int i = 0; true; i++)
                        {
                            if (i > ChunkAttempts)
                                throw new Exception("Reached maximum attempts while attempting to load chunk");
                            if (TryUploadChunk(location, originalBytes, start, end, total))
                                break;
                        }

                    }
                }
            }
        }

        public bool TryUploadChunk(string location, byte[] data, long start, long end, long total)
        {
            long contentLength = end - start + 1;
            if (contentLength != data.Length)
                throw new Exception("Data length does not match the provided range");
            try
            {
                var contentHeader = $"bytes {start}-{end}/{total}";
                client.Headers.Remove(HttpRequestHeader.ContentType);
                client.Headers[HttpRequestHeader.ContentLength] = contentLength.ToString();
                client.Headers[HttpRequestHeader.ContentRange] = contentHeader;
                var response = client.UploadData(location, "PUT", data);
                return true;
            }
            catch (WebException ex)
            {
                StreamReader sr = new StreamReader(ex.Response.GetResponseStream());
                var res = sr.ReadToEnd();
                var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                if (statusCode == System.Net.HttpStatusCode.PermanentRedirect)
                    return true;
                return false;
            }
        }
    }
}
