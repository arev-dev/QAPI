namespace QAPI.Models.DTO
{
    public class UserResponseModel
    {
        public int ResponseCode { get; set; }
        public User? User {get; set;} = new User();
        public List<User> Users { get; set; } = new List<User>();
    }
}
