namespace Application.Services.IAuthServices;

public interface IPassHashService
{
    void Create(string password, out byte[] PassHash, out byte[] PassSalt);
    bool ConfirmPasswordHash(string password, byte[] PassHash, byte[] PassSalt);
}
