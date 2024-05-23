using AutoMapper;
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
        private readonly IMapper _mapper;

        public AddressService(
            IAddressRepository addressRepo,
            IUserRepository userRepo,
            IMapper mapper
        )
        {
            _addressRepo = addressRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<AddressReadDto> CreateAddressAsync(Guid userId,AddressCreateDto address)
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

            var addressCreate=new Address{
                UserId=userId,
                FirstName=address.FirstName,
                LastName=address.LastName,
                AddressLine=address.AddressLine,
                PostalCode=address.PostalCode,
                PhoneNumber=address.PhoneNumber,
                Country=address.Country,
            };

            var newAddress= await _addressRepo.CreateAddressAsync(addressCreate);
            return _mapper.Map<AddressReadDto>(newAddress);
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

        public async Task<IEnumerable<AddressReadDto>> GetAllUserAddressesAsync(Guid userId)
        {
            var addresses = await _addressRepo.GetAllUserAddressesAsync(userId);
            return _mapper.Map<IEnumerable<AddressReadDto>>(addresses);
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
