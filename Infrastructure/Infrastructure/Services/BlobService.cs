using Application.Models.DTOs.Blob;
using Application.Services;

namespace Infrastructure.Services;

public class BlobService : IBlobService
{
    private readonly BlobStorageConfiguration _blobConfiguration;

    public BlobService(BlobStorageConfiguration blobConfiguration)
    {
        _blobConfiguration = blobConfiguration;
    }

    public async Task<Stream> GetBlobAsync(string containerName, string blobName)
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
