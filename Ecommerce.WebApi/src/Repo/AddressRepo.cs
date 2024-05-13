using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepositoryAbstraction;

namespace Ecommerce.WebApi.src.Repo
{
    public class AddressRepo : IAddressRepository
    {
        public Task<Address> CreateAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAddressByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Address> GetAddressByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Address>> GetAllUserAddressesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }
    }
}