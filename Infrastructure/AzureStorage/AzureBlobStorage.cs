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

    public async Task<Stream> GetAsync(string path)
    {
        var container = await GetContainerClientAsync();

        var blob = container.GetBlobClient(path);
        var blobExists = await blob.ExistsAsync();

        if (!blobExists)
        {
            throw new InfrastructureException($"Blob with path {path} is not in the blobs");
        }

        var download = await blob.DownloadAsync();
        var stream = download.Value.Content;

        return stream;
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