using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.WebApi.src.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebApi.src.Repo
{
    public class UserRepo : IUserRepository
    {
        private readonly EcommerceDbContext _context;
        private readonly DbSet<User> _users;
        private readonly IMapper _mapper;

        public UserRepo(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _users = _context.Users;
            _mapper = mapper;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            await _users.Where(p => p.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(QueryOptions options)
        {
            return await _users.ToListAsync(); // leave it like this for now. Add logic later
        }

        public async Task<User> GetUserByCredentialAsync(UserCredential credential)
        {
            var foundUser = await _users.FirstOrDefaultAsync(user => user.Email == credential.Email && user.Password == credential.Password);
            return foundUser;
        }

        public async Task<User>? GetUserByIdAsync(Guid id)
        {
            return await _users.SingleAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateUserByIdAsync(User user)
        {
            await _users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(setter =>
                    setter
                        .SetProperty(u => u.Email, user.Email)
                        .SetProperty(u => u.Avatar, user.Avatar)
                        .SetProperty(u => u.FirstName, user.FirstName)
                        .SetProperty(u => u.Password, user.Password)
                        .SetProperty(u => u.LastName, user.LastName)
                );
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _users.AnyAsync(user => user.Email == email);
        }
    }
}
