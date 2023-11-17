namespace Application.Services;

public interface IBlobService
{
    Task<bool> UploadBlobAsync(string containerName, string blobName, Stream content);
    Task<bool> RemoveBlobAsync(string containerName, string blobName);
    Task<Stream> GetBlobAsync(string containerName, string blobName);
}
