using Application.DTOs.Result;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IAWSS3Service
{
    Task<OperationResult<string>> UploadImage(string bucket, string key, IFormFile file);
}