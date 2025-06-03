using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Application.DTOs.Result;
using Application.Interfaces;
using Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AWSS3Service : IAWSS3Service
{
    private readonly AWSSettings configAWS;
    public BasicAWSCredentials AwsCredentials { get; set; }

    private readonly IAmazonS3 _awsS3Client;

    public AWSS3Service(IOptions<AWSSettings> options)
    {
        configAWS = options.Value;

        AwsCredentials = new BasicAWSCredentials(configAWS.AwsKeyId, configAWS.AwsKeySecret);

        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast2
        };

        _awsS3Client = new AmazonS3Client(AwsCredentials, config);
    }

    public async Task<OperationResult<string>> UploadImage(string bucket, string key, IFormFile file)
    {
        try
        {
            using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);

            var fileTransfer = new TransferUtility(_awsS3Client);

            await fileTransfer.UploadAsync(new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = key,
                BucketName = bucket,
                ContentType = file.ContentType
            });

            return OperationResult<string>.Success("Imagem enviada com sucesso ao S3.");
        }
        catch
        {
            return OperationResult<string>.Failure("Ocorreu um erro ao subir imagem para o S3.");
        }
    }
}