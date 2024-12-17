namespace LibraryManagementSystem.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(string username);
    }
}
