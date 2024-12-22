
using QAPI.Models;
using QAPI.Models.DTO;

namespace QAPI.Services.Interfaces;
public interface IUserService
{
    User GetUserById(int id);
    User GetUserByUsername(string username);
    List<User> GetAllUsers();
    UserResponseModel CreateUser(User user);
    UserResponseModel UpdateUser(User user);
    UserResponseModel DeleteUser(int id);
    UserResponseModel Login(User user);
}
