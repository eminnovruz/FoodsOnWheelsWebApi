namespace Application.Services.IHelperServices;

public interface IBlobService
{
    string GetSignedUrl(string fileName);
    bool UploadFile(Stream stream, string fileName, string contentType);
    Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType);
    bool DeleteFile(string fileName);
    Task<bool> DeleteFileAsync(string fileName);
}