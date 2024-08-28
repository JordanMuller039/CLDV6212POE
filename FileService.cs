using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ST10150702_CLDV6212_POE
{
    public class FileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        public FileService(IConfiguration configuration)
        {
            // Initialize the ShareServiceClient using the Azure Storage connection string from configuration
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        // Method to upload a file to Azure File Share
        public async Task UploadFileAsync(string shareName, string fileName, Stream content)
        {
            // Get a reference to the share (creates the share if it does not exist)
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync();

            // Get the root directory of the file share
            var directoryClient = shareClient.GetRootDirectoryClient();

            // Get the file client for the specified file name
            var fileClient = directoryClient.GetFileClient(fileName);

            // Create the file with the size of the content
            await fileClient.CreateAsync(content.Length);

            // Upload the content to the file
            await fileClient.UploadAsync(content);
        }

        // Method to list all files in the root directory of a file share
        public async Task<List<string>> ListFilesAsync(string shareName)
        {
            // Get a reference to the share (creates the share if it does not exist)
            var shareClient = _shareServiceClient.GetShareClient(shareName);

            // Get the root directory of the file share
            var directoryClient = shareClient.GetRootDirectoryClient();

            var fileList = new List<string>();

            // List all files and directories in the root directory
            await foreach (ShareFileItem item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                // Only add files to the list (IsDirectory is false for files)
                if (!item.IsDirectory)
                {
                    fileList.Add(item.Name);
                }
            }

            return fileList;
        }

        // Method to delete a file from the file share
        public async Task DeleteFileAsync(string shareName, string fileName)
        {
            // Get a reference to the share
            var shareClient = _shareServiceClient.GetShareClient(shareName);

            // Get the root directory of the file share
            var directoryClient = shareClient.GetRootDirectoryClient();

            // Get the file client for the specified file name
            var fileClient = directoryClient.GetFileClient(fileName);

            // Delete the file if it exists
            await fileClient.DeleteIfExistsAsync();
        }
    }
}
