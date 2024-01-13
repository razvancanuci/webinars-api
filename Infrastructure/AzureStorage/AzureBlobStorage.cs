using Azure.Storage.Blobs;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AzureStorage;

public class AzureBlobStorage : IFileStorage
{
    private readonly AzureStorageSettings _settings;
    public AzureBlobStorage(IOptions<AzureStorageSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<Uri?> GetAsync(string path)
    {
        var container = await GetContainerClientAsync();

        var blob = container.GetBlobs(prefix: path).FirstOrDefault();
        
        if (blob is null)
        {
            return default;
        }
        
        var blobContent = container.GetBlobClient(blob.Name);

        return blobContent.Uri;
    }

    public async Task CreateAsync(string path, IFormFile blob)
    {
        var container = await GetContainerClientAsync();

        var blobClient = container.GetBlobClient(path);

        await using Stream stream = blob.OpenReadStream();
        
        await blobClient.UploadAsync(stream, overwrite: true);
    }
    
    private async Task<BlobContainerClient> GetContainerClientAsync()
    {
        var client = new BlobServiceClient(_settings.ConnectionString);

        var container = client.GetBlobContainerClient(_settings.ContainerName);

        var containerExists = await container.ExistsAsync();

        if (!containerExists)
        {
            await container.CreateIfNotExistsAsync();
        }

        return container;
    }
    
}