using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class AddressRepo : IAddressRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<Address> _addresses;

        public AddressRepo(EcommerceDbContext context)
        {
            _context = context;
            _addresses = context.Addresses;
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            await _addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressByIdAsync(Guid id)
        {
            await _addresses.Where(a => a.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> GetAddressByIdAsync(Guid id)
        {
            return await _addresses.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Address>> GetAllUserAddressesAsync(Guid userId)
        {
            return await _addresses.ToListAsync();
        }

        public Task<bool> UpdateAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
