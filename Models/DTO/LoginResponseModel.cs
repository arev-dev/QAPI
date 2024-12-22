namespace QAPI.Models.DTO
{
    public class LoginResponseModel
    {
        public int ResponseCode { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
