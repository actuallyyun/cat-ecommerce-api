using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Common;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IUserService
    {
        Task<UserReadDto> CreateUserAsync(UserCreateDto user);
        Task<bool> UpdateUserByIdAsync(Guid id,UserUpdateDto user);
        Task<UserReadDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync(QueryOptions options);
        Task<bool> DeleteUserByIdAsync(Guid id);
    }
}
