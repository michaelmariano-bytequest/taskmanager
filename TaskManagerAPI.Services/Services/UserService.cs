using AutoMapper;
using TaskManagerAPI.Core.DTOs;
using TaskManagerAPI.Core.Entities;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDTO> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<UserDTO>(user);
    }

    public async Task CreateUserAsync(CreateUserDTO createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        user.PasswordHash = HashPassword(createUserDto.Password);
        await _userRepository.CreateUserAsync(user);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}