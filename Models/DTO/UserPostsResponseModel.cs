namespace QAPI.Models.DTO
{
    public class UserPostsResponseModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ResponseCode { get; set; }
    }
}
