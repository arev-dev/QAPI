namespace QAPI.Models.DTO
{
    public class PostResponseModel
    {
        public int ResponseCode { get; set; }
        public Post? Post {get; set;} = new Post();
        public List<Post> Users { get; set; } = new List<Post>();
    }
}
