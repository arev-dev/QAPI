namespace QAPI.Models.DTO
{
    public class CommentResponseModel
    {
        public int ResponseCode { get; set; }
        public Comment? Comment {get; set;} = new Comment();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
