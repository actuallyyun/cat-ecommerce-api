using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Ecommerce.Service.src.Validation;

namespace Ecommerce.Service.src.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto userDto)
        {
            UserValidation.ValidateUserCreateDto(userDto);

            var user = _mapper.Map<User>(userDto);

            var isUserExistWithEmail = await _userRepo.UserExistsByEmailAsync(user.Email);
            
            if (isUserExistWithEmail )
            {
                throw new ArgumentException($"A user with the email {user.Email} already exists.");
            }

            var createdUser = await _userRepo.CreateUserAsync(user);

            return _mapper.Map<UserReadDto>(createdUser);
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);
           
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return await _userRepo.DeleteUserByIdAsync(id);
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(QueryOptions options)
        {
            var users = await _userRepo.GetAllUsersAsync(options);

            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<UserReadDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<bool> UpdateUserByIdAsync(Guid id, UserUpdateDto userDto)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            UserValidation.ValidateUserUpdateDto(userDto);

            _mapper.Map(userDto, existingUser);

            return await _userRepo.UpdateUserByIdAsync(existingUser);
        }
    }
}
