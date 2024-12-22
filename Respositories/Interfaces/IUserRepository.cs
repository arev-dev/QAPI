using QAPI.Models.DTO;

namespace QAPI.Models;
public interface IUserRepository
{
    User GetUserById(int id);
    User GetUserByUsername(string username);
    List<User> GetAllUsers();
    UserResponseModel CreateUser(User user);
    UserResponseModel UpdateUser(User user);
    UserResponseModel DeleteUser(int id);
    LoginResponseModel Login(User user);
}