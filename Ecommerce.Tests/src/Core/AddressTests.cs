using Ecommerce.Core.src.Entity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Tests.src.Core
{
    public class AddressTests
    {

        private void ValidateAddress(Address address)
        {
            var context = new ValidationContext(address);
            Validator.ValidateObject(address, context, validateAllProperties: true);
        }
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            var userId = Guid.NewGuid();
            var addressLine = "123 Main St";
            var postalCode = "90210";
            var country = "USA";
            var phoneNumber = "1234567890";

            var address = new Address(userId, addressLine, postalCode, country, phoneNumber);

            Assert.Equal(userId, address.UserId);
            Assert.Equal(addressLine, address.AddressLine);
            Assert.Equal(postalCode, address.PostalCode);
            Assert.Equal(country, address.Country);
            Assert.Equal(phoneNumber, address.PhoneNumber);
        }
        [Fact]
        public void Constructor_WithInvalidPostalCode_ShouldThrowValidationException()
        {
            var userId = Guid.NewGuid();
            var addressLine = "123 Main St";
            var postalCode = ""; // Invalid postal code (empty)
            var country = "USA";
            var phoneNumber = "123-456-7890";

            var address = new Address(userId, addressLine, postalCode, country, phoneNumber);

            Assert.Throws<ValidationException>(() => ValidateAddress(address));
        }



        [Fact]
        public void UpdateAddress_WithEmptyCountry_ShouldThrowValidationException()
        {
            var userId = Guid.NewGuid();
            var address = new Address(userId, "123 Main St", "90210", "USA", "123-456-7890");

            address.Country = "";

            Assert.Throws<ValidationException>(() => ValidateAddress(address));
        }

        [Fact]
        public void Constructor_WithEmptyAddressLine_ShouldThrowValidationException()
        {
            var userId = Guid.NewGuid();
            var addressLine = ""; // Invalid (empty string)
            var postalCode = "90210";
            var country = "USA";
            string phoneNumber = "555-1234"; 

            var address = new Address(userId, addressLine, postalCode, country, phoneNumber);

            Assert.Throws<ValidationException>(() => ValidateAddress(address));
        }
    }
}
