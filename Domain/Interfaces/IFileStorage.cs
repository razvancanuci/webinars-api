using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IFileStorage
{
    Task<(Stream Stream,string ContentType)> GetAsync(string path, CancellationToken cancellationToken = default);
    Task CreateAsync(string path, IFormFile content, CancellationToken cancellationToken = default);
}