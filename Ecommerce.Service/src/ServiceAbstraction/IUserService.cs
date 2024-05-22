using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstraction
{
    public interface IUserService
    {
        Task<UserReadDto> CreateUserAsync(UserCreateDto user);
        Task<bool> UpdateUserByIdAsync(Guid id, UserUpdateDto user);
        Task<UserReadDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid id);
        Task<IEnumerable<OrderReadDto>> GetAllUserOrdersAsync(Guid id);
        Task<IEnumerable<UserReadDto>> GetAllUserAsync(QueryOptions options);

        Task<bool> DeleteUserByIdAsync(Guid id);
    }
}
