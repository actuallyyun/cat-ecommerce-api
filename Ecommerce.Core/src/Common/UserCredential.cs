namespace Ecommerce.Core.src.Common
{
    public class UserCredential
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserCredential(string email,string password){
            Email=email;
            Password=password;// should hash it.
        }
    }
}
