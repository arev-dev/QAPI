namespace QAPI.Models.DTO
{
    public class UserCommentsPostResponseModel
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ResponseCode { get; set; }
    }
}
