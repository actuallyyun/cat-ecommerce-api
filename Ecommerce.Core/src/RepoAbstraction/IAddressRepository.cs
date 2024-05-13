using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepositoryAbstraction
{
    public interface IAddressRepository
    {
       Task<Address> CreateAddressAsync(Address address);
        Task<bool> UpdateAddressAsync(Address address);
        Task<Address> GetAddressByIdAsync(Guid id);
        Task<IEnumerable<Address>> GetAllUserAddressesAsync(Guid userId);
        Task<bool> DeleteAddressByIdAsync(Guid id);
    }
}