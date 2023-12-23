﻿using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces;

public interface IFileStorage
{
    Task<Stream> GetAsync(string path);
    Task CreateAsync(string path, IFormFile content);
}