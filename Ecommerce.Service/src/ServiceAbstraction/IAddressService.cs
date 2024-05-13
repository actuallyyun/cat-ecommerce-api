using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IAddressService
    {
        Task<AddressReadDto> CreateAddressAsync(AddressCreateDto address);
        Task<bool> UpdateAddressByIdAsync(Guid id, AddressUpdateDto address);
        Task<AddressReadDto> GetAddressByIdAsync(Guid id);
        Task<IEnumerable<AddressReadDto>> GetAllUserAddressesAsync(Guid userId);
        Task<bool> DeleteAddressByIdAsync(Guid id);
    }
}
