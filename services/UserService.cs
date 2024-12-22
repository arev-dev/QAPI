using QAPI.Models;
using QAPI.Models.DTO;
using QAPI.Services.Interfaces;

namespace QAPI.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public UserResponseModel CreateUser(User user)
    {
        return _userRepository.CreateUser(user);
    }

    public UserResponseModel DeleteUser(int id)
    {
        return _userRepository.DeleteUser(id);
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public User GetUserById(int id)
    {
        return _userRepository.GetUserById(id);
    }

    public User GetUserByUsername(string username)
    {
        return _userRepository.GetUserByUsername(username);
    }

    public LoginResponseModel Login(User user)
    {
        return _userRepository.Login(user);
    }

    public UserResponseModel UpdateUser(User user)
    {
        return _userRepository.UpdateUser(user);
    }
}
