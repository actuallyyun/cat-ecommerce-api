using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IAddressService
    {
        Task<Address> CreateAddressAsync(AddressCreateDto address);
        Task<bool> UpdateAddressByIdAsync(Guid id,Guid userId, AddressUpdateDto address);
        Task<Address> GetAddressByIdAsync(Guid addressId,Guid userId);
        Task<IEnumerable<Address>> GetAllUserAddressesAsync(Guid userId);
        Task<bool> DeleteAddressByIdAsync(Guid addressId,Guid userId);
    }
}
