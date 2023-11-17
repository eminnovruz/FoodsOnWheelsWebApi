using Application.Services;

namespace Infrastructure.Services;

public class BlobService : IBlobService
{
    public Task<Stream> GetBlobAsync(string containerName, string blobName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveBlobAsync(string containerName, string blobName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UploadBlobAsync(string containerName, string blobName, Stream content)
    {
        throw new NotImplementedException();
    }
}
