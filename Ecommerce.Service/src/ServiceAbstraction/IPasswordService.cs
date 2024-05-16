namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IPasswordService
    {
        string HashPassword(string password,out byte[]salt);
        bool VerifyPassword(string password,string passwordHash,byte[]salt);
    }
}