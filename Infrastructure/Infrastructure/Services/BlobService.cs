using Application.Models.DTOs.Blob;
using Application.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;

namespace Infrastructure.Services;

public class BlobService : IBlobService
{
    private readonly BlobStorageConfiguration _storageOptions;

    public BlobService(IOptions<BlobStorageConfiguration> blobOptions)
    {
        _storageOptions = blobOptions.Value;
    }

    public bool DeleteFile(string fileName)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        return blobClient.DeleteIfExists().Value;
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var response = await blobClient.DeleteIfExistsAsync();
        return response.Value;
    }

    public string GetSignedUrl(string fileName)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = contaionerClient.GetBlobClient(fileName);

        var signedUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.Now.AddMonths(1)).AbsoluteUri;

        return signedUrl;
    }

    public Task<string> GetSignedUrlAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public bool UploadFile(Stream stream, string fileName, string contentType)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = contaionerClient.GetBlobClient(fileName);

        blobClient.Upload(stream);
        return true;
    }

    public async Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = contaionerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(stream);
        return true;
    }
}
