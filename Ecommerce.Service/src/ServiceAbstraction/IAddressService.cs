using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IAddressService
    {
        Task<AddressReadDto> CreateAddressAsync(Guid userId,AddressCreateDto address);
        Task<bool> UpdateAddressByIdAsync(Guid id,Guid userId, AddressUpdateDto address);
        Task<Address> GetAddressByIdAsync(Guid addressId,Guid userId);
        Task<IEnumerable<AddressReadDto>> GetAllUserAddressesAsync(Guid userId);
        Task<bool> DeleteAddressByIdAsync(Guid addressId,Guid userId);
    }
}
