using Application.Models.DTOs.Blob;
using Application.Services.IHelperServices;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;

namespace Infrastructure.Services.HelperServices;

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

    public bool UploadFile(Stream stream, string fileName, string contentType)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = contaionerClient.GetBlobClient(fileName);
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

        blobClient.Upload(stream, blobHttpHeaders);

        return true;
    }

    public async Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = contaionerClient.GetBlobClient(fileName);
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

        await blobClient.UploadAsync(stream, blobHttpHeaders);
        
        return true;
    }
}
