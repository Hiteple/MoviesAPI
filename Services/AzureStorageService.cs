using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace MoviesAPI.Services
{
    public class AzureStorageService: IFileStorageService
    {
        private readonly string _connectionString;
        
        public AzureStorageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorageConnection");
        }
        
        public async Task<string> EditFile(byte[] content, string extension, string containerName, string fileRoute, string contentType)
        {
            await DeleteFile(fileRoute, containerName);
            return await SaveFile(content, extension, containerName, contentType);
        }

        public async Task DeleteFile(string fileRoute, string containerName)
        {
            if (fileRoute != null)
            {
                var account = CloudStorageAccount.Parse(_connectionString);
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference(containerName);

                // Get the blob name
                var blobName = Path.GetFileName(fileRoute);
            
                // Get blob reference
                var blob = container.GetBlobReference(blobName);
            
                // Delete the blob
                await blob.DeleteIfExistsAsync();
            }
        }

        public async Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType)
        {
            var account = CloudStorageAccount.Parse(_connectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(content, 0, content.Length);
            blob.Properties.ContentType = contentType;
            await blob.SetPropertiesAsync();
            return blob.Uri.ToString();
        }
    }
}