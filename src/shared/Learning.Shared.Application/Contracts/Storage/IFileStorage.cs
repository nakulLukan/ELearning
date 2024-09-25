using Learning.Shared.Common.Dto.Storage;

namespace Learning.Shared.Application.Contracts.Storage;

public interface IFileStorage
{
    /// <summary>
    /// Uploads the file to the storage.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(string SignedUrl, string RelativePath)> UploadFile(
        byte[] file,
        string fileName,
        string filePath,
        CancellationToken cancellationToken);

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
        CancellationToken cancellationToken);

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
        CancellationToken cancellationToken);

    /// <summary>
    /// Uploads the file to the storage.
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(string SignedUrl, string RelativePath)> UploadFile(
        Stream fileStream,
        string fileName,
        string filePath,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileName, string filePath);

    /// <summary>
    /// Gets a signed url with low lifetime.
    /// </summary>
    /// <param name="fileRelativePath"></param>
    /// <returns></returns>
    public string GetPresignedUrl(string fileRelativePath);

    /// <summary>
    /// Delete file from storage
    /// </summary>
    /// <param name="fullFilePath"></param>
    /// <returns></returns>
    public Task<bool> DeleteFileAsync(string fullFilePath);

    /// <summary>
    /// Get file details under a given path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Task<List<FileDetailDto>> ListFilesAsync(string path);

    /// <summary>
    /// Get object url
    /// </summary>
    /// <param name="relativePath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string GetObjectUrl(string relativePath, string fileName);

    /// <summary>
    /// Get object url
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public string GetObjectUrl(string relativePath);

    public Task CreateDirectory(string directoryRelativePath);
    public string GetS3ConsoleLink(string relativePath);
    public Task<List<string>> GetFileNames(string relativePath);
}
