
using QAPI.Models;

namespace QAPI.Services.Interfaces;
public interface IUserService
{
    User GetUserById(int id);
    int CreateUser(User user);
}
