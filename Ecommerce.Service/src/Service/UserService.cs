using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstraction;
using Ecommerce.Core.src.RepositoryAbstraction;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Ecommerce.Service.src.Validation;

namespace Ecommerce.Service.src.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, IReviewRepository reviewRepo,IOrderRepository orderRepo,IPasswordService passwordService,IMapper mapper)
        {
            _userRepo = userRepo;
            _reviewRepo=reviewRepo;
            _orderRepo=orderRepo;
            _passwordService=passwordService;
            _mapper = mapper;
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto userDto)
        {
            UserValidation.ValidateUserCreateDto(userDto);
                        
            var user = _mapper.Map<User>(userDto);
            
            user.Password=_passwordService.HashPassword(userDto.Password,out byte[]salt);
            user.Salt=salt;    
            var createdUser = await _userRepo.CreateUserAsync(user);

            return _mapper.Map<UserReadDto>(createdUser);
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

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid id){
            return await _reviewRepo.GetReviewsByUserIdAsync(id);
        }

        public async Task<bool> UpdateUserByIdAsync(Guid id, UserUpdateDto userDto)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            UserValidation.ValidateUserUpdateDto(userDto);

            if(userDto.FirstName!=null){
                existingUser.FirstName = userDto.FirstName;
            }
            if(userDto.LastName!=null){
                existingUser.LastName=userDto.LastName;
            }
            if(userDto.Avatar!=null){
                existingUser.Avatar=userDto.Avatar;
            }
            if(userDto.Password!=null){
                var hashedPassword=_passwordService.HashPassword(userDto.Password,out byte[]salt);
                existingUser.Password=hashedPassword;
                existingUser.Salt=salt;
            }

            return await _userRepo.UpdateUserByIdAsync(existingUser);
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            var existingUser = await _userRepo.GetUserByIdAsync(id);

            if (existingUser == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }

            return await _userRepo.DeleteUserByIdAsync(id);
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllUserOrdersAsync(Guid id)//get orders of a user
        {
            var orders= await _orderRepo.GetAllUserOrdersAsync(id);
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUserAsync(QueryOptions options)
        {
        var users=await _userRepo.GetAllUsersAsync(options);
        return 
        _mapper.Map<IEnumerable<UserReadDto>>(users);
        }
    }
}
