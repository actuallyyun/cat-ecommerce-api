using System.Text.RegularExpressions;

namespace Ecommerce.Service.src.Validation
{
    public class CustomValidator
    {
        public CustomValidator(){}
        private Regex _validatePhoneNumberRegex = new Regex("^\\+?[1-9][0-9]{7,14}$");

        public bool ValidatePhoneNumber(string phoneNumber){
            return _validatePhoneNumberRegex.IsMatch(phoneNumber);
        }

        
    }
}