﻿using Amazon.S3;
using Amazon.S3.Model;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Learning.Shared.Infrastructure.Impl.Storage;

public class AwsStorage : IFileStorage
{
    readonly ILogger<AwsStorage> _logger;
    AmazonS3Client _client;
    string _bucketName;
    string _region;

    public AwsStorage(IConfiguration configuration, ILogger<AwsStorage> logger)
    {
        _client = InitialiseAwsClient(configuration);
        _bucketName = configuration["Aws:S3:BucketName"];
        _logger = logger;
    }

    /// <summary>
    /// Uploads the file to the public storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(string SignedUrl, string RelativePath)> UploadFileToPublic(
        byte[] file,
        string fileName,
        string filePath,
        CancellationToken cancellationToken)
    {
        if (!filePath.StartsWith(StoragePathConstant.PUBLIC))
        {
            filePath = $"{StoragePathConstant.PUBLIC}/{filePath}";
        }

        return UploadFile(file, fileName, filePath, cancellationToken);
    }

    /// <summary>
    /// Uploads the file to the private storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(string SignedUrl, string RelativePath)> UploadFileToPrivate(
        byte[] file,
        string fileName,
        string filePath,
        CancellationToken cancellationToken)
    {
        if (!filePath.StartsWith(StoragePathConstant.PRIVATE))
        {
            filePath = $"{StoragePathConstant.PRIVATE}/{filePath}";
        }

        return UploadFile(file, fileName, filePath, cancellationToken);
    }

    /// <summary>
    /// Uploads the file to the storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(string SignedUrl, string RelativePath)> UploadFile(
        byte[] file,
        string fileName,
        string filePath,
        CancellationToken cancellationToken)
    {
        PutObjectRequest putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = RelativePath(filePath, fileName),
            InputStream = new MemoryStream(file), // Convert byte array to stream
        };

        PutObjectResponse response = await _client.PutObjectAsync(putRequest, cancellationToken);
        _logger.LogInformation("Filename: {filename}, File upload status {status}", RelativePath(filePath, fileName), response.HttpStatusCode);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError("Failed to upload file. Reason: {reason}", response.ToString());
        }
        string signedUrl = GetPresignedUrl(fileName, filePath);
        return (signedUrl, RelativePath(filePath, fileName));
    }

    /// <summary>
    /// Uploads the file to the storage.
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(string SignedUrl, string RelativePath)> UploadFile(
        Stream fileStream,
        string fileName,
        string filePath,
        CancellationToken cancellationToken)
    {
        PutObjectRequest putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = RelativePath(filePath, fileName),
            InputStream = fileStream,
        };

        PutObjectResponse response = await _client.PutObjectAsync(putRequest, cancellationToken);
        _logger.LogInformation("Filename: {filename}, File upload status {status}", RelativePath(filePath, fileName), response.HttpStatusCode);
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            _logger.LogError("Failed to upload file. Reason: {reason}", response.ToString());
        }
        string signedUrl = GetPresignedUrl(fileName, filePath);
        return (signedUrl, RelativePath(filePath, fileName));
    }

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileName, string filePath)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = RelativePath(filePath, fileName),
            Expires = DateTime.Now.AddMinutes(30) // Set expiration time for the URL
        };

        string signedUrl = _client.GetPreSignedURL(request);
        return signedUrl;
    }

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileRelativePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileRelativePath)
    {
        if (string.IsNullOrEmpty(fileRelativePath))
        {
            return string.Empty;
        }

        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileRelativePath,
            Expires = DateTime.Now.AddMinutes(30) // Set expiration time for the URL
        };

        string signedUrl = _client.GetPreSignedURL(request);
        return signedUrl;
    }

    public async Task<bool> DeleteFileAsync(string fullFilePath)
    {
        var response = await _client.DeleteObjectAsync(new()
        {
            BucketName = _bucketName,
            Key = fullFilePath,
        });

        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    /// <summary>
    /// Get object url
    /// </summary>
    /// <param name="relativePath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string GetObjectUrl(string relativePath, string fileName)
    {
        if (!relativePath.StartsWith(StoragePathConstant.PUBLIC))
        {
            relativePath = $"{StoragePathConstant.PUBLIC}/{relativePath}";
        }
        return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{relativePath}/{fileName}";
    }

    /// <summary>
    /// Get object url
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public string GetObjectUrl(string relativePath)
    {
        return GetObjectUrl(Path.GetDirectoryName(relativePath), Path.GetFileName(relativePath));
    }

    /// <summary>
    /// Get file details under a given path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<List<FileDetailDto>> ListFilesAsync(string path)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = path
        };

        var response = await _client.ListObjectsV2Async(request);

        return response.S3Objects.Select(obj => new FileDetailDto
        {
            FileName = obj.Key,
            LastModified = obj.LastModified,
            FileSize = obj.Size,
        }).ToList();
    }


    private AmazonS3Client InitialiseAwsClient(IConfiguration configuration)
    {
        string accessKey = configuration["Aws:S3:AccessKey"];
        string secretKey = configuration["Aws:S3:SecretKey"];
        _region = configuration["Aws:S3:Region"];
        bool useBasicCredentialAuthentication = bool.Parse(configuration["Aws:S3:UseBasicCredentialAuthentication"]);
        if (useBasicCredentialAuthentication)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            return new AmazonS3Client(credentials, Amazon.RegionEndpoint.GetBySystemName(_region));
        }
        else
        {
            return new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(_region));
        }
    }

    private static string RelativePath(string path, string fileName)
    {
        if (string.IsNullOrEmpty(path))
        {
            return fileName;
        }

        return path + "/" + fileName;
    }

    public async Task CreateDirectory(string directoryRelativePath)
    {
        if (!directoryRelativePath.EndsWith("/"))
        {
            directoryRelativePath += "/";
        }

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = directoryRelativePath, // The key represents the directory.
            ContentBody = string.Empty // S3 requires some content for a PUT request, even if it's an empty directory.
        };

        var response = await _client.PutObjectAsync(putRequest);
    }

    public string GetS3ConsoleLink(string relativePath)
    {
        if (!relativePath.EndsWith("/"))
        {
            relativePath += "/";
        }
        return $"https://s3.console.aws.amazon.com/s3/buckets/{_bucketName}?region={_region}&bucketType=general&prefix={Uri.EscapeDataString(relativePath)}&showversions=false";
    }

    public async Task<List<string>> GetFileNames(string relativePath)
    {
        var files = new List<string>();

        if (!relativePath.EndsWith("/"))
        {
            relativePath += "/";
        }

        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = relativePath // The prefix acts as the directory name in S3.
        };

        ListObjectsV2Response response;

        do
        {
            response = await _client.ListObjectsV2Async(request);

            foreach (S3Object entry in response.S3Objects)
            {
                files.Add(Path.GetFileName(entry.Key)); // Adding each file's key (path) to the list.
            }

            // If response is truncated, continue to fetch more keys.
            request.ContinuationToken = response.NextContinuationToken;
        }
        while (response.IsTruncated);

        return files;
    }
}
