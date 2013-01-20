using Microsoft.Live;
using SkyDriveHelpers.WP8.Exceptions;
using SkyDriveHelpers.WP8.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDriveHelpers.WP8
{
    public class SkyDriveHelper
    {
        LiveConnectSession _session;
        LiveConnectClient _client;

        public SkyDriveHelper()
        {
                
        }

        public SkyDriveHelper(LiveConnectSession session)
        {
            _session = session;
        }

        public void SetSession(LiveConnectSession session)
        {
            _session = session;
        }

        public async Task<LiveUserInfo> GetLiveUserInfo()
        {
            LiveUserInfo userInfo = new LiveUserInfo();
            if (!await ValidateClient())
            {
                return null;
            }

            LiveOperationResult result = await _client.GetAsync("me"); // was "me"

            dynamic meResult = result.Result;
            userInfo.FirstName = meResult.first_name;
            userInfo.LastName = meResult.last_name;

            return userInfo;
        }

        public async Task<List<DirectoryEntry>> GetDirectoryEntries(string path)
        {
            if (!await ValidateClient())
            {
                return null;
            }

            List<DirectoryEntry> results = new List<DirectoryEntry>();

            string skydrivePath = string.Format("me/skydrive{0}", string.IsNullOrWhiteSpace(path) ? "/" : path);
            if (!skydrivePath.EndsWith("/"))
            {
                skydrivePath += "/";
            }
            skydrivePath += "files";

            LiveOperationResult operationResult = await _client.GetAsync(skydrivePath);

            List<object> data = (List<object>)operationResult.Result["data"];
            foreach (IDictionary<string, object> content in data)
            {
                DirectoryEntry de = new DirectoryEntry();

                de.Description    = content["description"] as string;
                de.From           = content["from"] as string;
                de.Id             = content["id"] as string;
                de.Link           = content["link"] as string;
                de.Name           = content["name"] as string;
                de.ParentId       = content["parent_id"] as string;
                de.Type           = content["type"] as string;
                de.UploadLocation = content["upload_location"] as string;

                long size = 0;
                long.TryParse(content["size"] as string, out size);
                de.Size = size;

                results.Add(de);
            }

            return results;
        }

        public async Task<DirectoryEntry> GetDirectoryEntry(string id)
        {
            if (!await ValidateClient())
            {
                return null;
            }

            LiveOperationResult operationResult = await _client.GetAsync(id);

            dynamic content = operationResult.Result;

            DirectoryEntry de = new DirectoryEntry();

            de.Description    = content.description as string;
            de.From           = content.from as string;
            de.Id             = content.id as string;
            de.Link           = content.link as string;
            de.Name           = content.name as string;
            de.ParentId       = content.parent_id as string;
            de.Type           = content.type as string;
            de.UploadLocation = content.upload_location as string;

            long size = 0;
            long.TryParse(content.size as string, out size);
            de.Size = size;

            return de;
        }

        public async Task<Stream> DownloadFile(string id)
        {
            if (!await ValidateClient())
            {
                return null;
            }

            string skydriveFilePath = string.Format("{0}/content", id);

            var operationResult = await _client.DownloadAsync(skydriveFilePath);

            return operationResult.Stream;
        }

        public async void DownloadFile(string id, string fileLocation)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            
            using(Stream stream = await DownloadFile(id))
            {
                using (IsolatedStorageFileStream fs = store.CreateFile(fileLocation))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;

                    do
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, 1024);
                        if (bytesRead > 0)
                        {
                            await fs.WriteAsync(buffer, 0, bytesRead);
                        }
                    } while (bytesRead > 0);

                    fs.Close();
                    stream.Close();
                }
            }
            return;
        }


        /// <summary>
        /// Validates the internal _client is valid and constructed with a non expired session.
        /// </summary>
        /// <returns>True - client is good.  False - client is bad.</returns>
        private async Task<bool> ValidateClient()
        {
            if (_client == null || _client.Session == null)
            {
                _client = new LiveConnectClient(_session);
                return true;
            }

            if (_client.Session.Expires < DateTime.Now)
            {
                return false;
                //throw new SessionExpiredException();
            }

            return true;
        }


    }
}


/*

        private async void btnLiveSignIn_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == LiveConnectSessionStatus.Connected)
            {
                liveClient = new LiveConnectClient(e.Session);
                viewModel.Session = e.Session;

                LiveOperationResult result = await liveClient.GetAsync("me");

                try
                {
                    dynamic meResult = result.Result;
                    if (meResult.first_name != null &&
                        meResult.last_name != null)
                    {
                        viewModel.Message = "Hello " +
                            meResult.first_name + " " +
                            meResult.last_name + "!";
                    }
                    else
                    {
                        viewModel.Message = "Hello, signed-in user!";
                    }

                    LiveOperationResult mydocs = await liveClient.GetAsync("me/skydrive/my_documents");
                    dynamic mydocres = mydocs.Result; //mydocres.id
                    var myfolder = await liveClient.GetAsync(string.Format("me/skydrive/my_documents/files"));

                    List<object> data = (List<object>)myfolder.Result["data"];
                    foreach (IDictionary<string, object> content in data)
                    {
                        string type = (string)content["type"];
                        if (type == "folder")
                        {
                            // do something with folders?
                        }
                        
                        string filename = (string)content["name"];
                        string fileId = (string)content["id"];

                        if (filename.ToUpper().EndsWith(".PSAFE3"))
                        {
                            viewModel.Files.Add(filename);
                            viewModel.SelectedFile = filename;
                            viewModel.FileToken    = fileId;
                            viewModel.Filename     = filename;
                            viewModel.IsSkydriveContent = true;
                        }
                    }
                    
                }
                catch (LiveConnectException exception)
                {
                    viewModel.Message = "Error calling API: " +
                        exception.Message;
                }


            }
            else
            {
                viewModel.Message = "Not signed in.";
            }
        }
*/
