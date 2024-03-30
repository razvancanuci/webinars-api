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

    public async Task<Uri?> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        var container = await GetContainerClientAsync(cancellationToken);

        var blob = container.GetBlobs(prefix: path, cancellationToken: cancellationToken).FirstOrDefault();
        
        if (blob is null)
        {
            return default;
        }
        
        var blobContent = container.GetBlobClient(blob.Name);

        return blobContent.Uri;
    }

    public async Task CreateAsync(string path, IFormFile blob, CancellationToken cancellationToken = default)
    {
        var container = await GetContainerClientAsync(cancellationToken);

        var blobClient = container.GetBlobClient(path);

        await using Stream stream = blob.OpenReadStream();
        
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
    }
    
    private async Task<BlobContainerClient> GetContainerClientAsync(CancellationToken cancellationToken = default)
    {
        var client = new BlobServiceClient(_settings.ConnectionString);

        var container = client.GetBlobContainerClient(_settings.ContainerName);

        var containerExists = await container.ExistsAsync(cancellationToken);

        if (!containerExists)
        {
            await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }

        return container;
    }
    
}