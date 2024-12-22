using QAPI.Models;
using QAPI.Services.Interfaces;

namespace QAPI.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public int CreateUser(User user)
    {
        return _userRepository.CreateUser(user);
    }

    public User GetUserById(int id)
    {
        return _userRepository.GetUserById(id);
    }
}
