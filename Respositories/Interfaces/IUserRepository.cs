namespace QAPI.Models;
public interface IUserRepository
{
    User GetUserById(int id);
    int CreateUser(User user);
}