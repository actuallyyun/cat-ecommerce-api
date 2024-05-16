using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Ecommerce.Service.src.Validation;

namespace Ecommerce.Service.src.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepo;
        private readonly IUserRepository _userRepo;

        public AddressService(IAddressRepository addressRepo, IUserRepository userRepo)
        {
            _addressRepo = addressRepo;
            _userRepo = userRepo;
        }

        public async Task<Address> CreateAddressAsync(AddressCreateDto address)
        {
            var validator = new CustomValidator();
            if (address.PhoneNumber != null)
            {
                var isValidNumber = validator.ValidatePhoneNumber(address.PhoneNumber);
                if (!isValidNumber)
                {
                    throw new InvalidDataException("Invalid phone number");
                }
            }

            var newAddress = new Address(
                address.UserId,
                address.AddressLine,
                address.PostalCode,
                address.Country,
                address.PhoneNumber
            );
            return await _addressRepo.CreateAddressAsync(newAddress);
        }

        public async Task<bool> UpdateAddressByIdAsync(
            Guid id,
            Guid userId,
            AddressUpdateDto address
        )
        {
            await IsOwner(id, userId);

            var validator = new CustomValidator();
            var isValidNumber = validator.ValidatePhoneNumber(address.PhoneNumber);
            if (!isValidNumber)
            {
                throw new InvalidDataException("Invalid phone number");
            }
            var addressFound = await _addressRepo.GetAddressByIdAsync(id);
            try
            {
                addressFound.AddressLine = address.AddressLine ?? addressFound.AddressLine;
                addressFound.Country = address.Country ?? addressFound.Country;
                addressFound.PhoneNumber = address.PhoneNumber ?? addressFound.PhoneNumber;
                addressFound.PostalCode = address.PostalCode ?? addressFound.PostalCode;
                addressFound.UpdatedAt = DateTime.UtcNow;
            }
            catch (Exception)
            {
                throw;
            }
            return await _addressRepo.UpdateAddressAsync(addressFound);
        }

        public async Task<bool> DeleteAddressByIdAsync(Guid addressId, Guid userId)
        {
            await IsOwner(addressId, userId);
            return await _addressRepo.DeleteAddressByIdAsync(addressId);
        }

        public async Task<Address> GetAddressByIdAsync(Guid addressId, Guid userId)
        {
            await IsOwner(addressId, userId);
            return await _addressRepo.GetAddressByIdAsync(addressId);
        }

        public async Task<IEnumerable<Address>> GetAllUserAddressesAsync(Guid userId)
        {
            return await _addressRepo.GetAllUserAddressesAsync(userId);
        }

        private async Task IsOwner(Guid addressId, Guid userId)
        {
            var address = await _addressRepo.GetAddressByIdAsync(addressId);
            if (address == null)
            {
                throw new ArgumentException($"Address with {addressId} does not exist");
            }
            if (address.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
