using QAPI.Models;
using QAPI.Models.DTO;

namespace QAPI.Services.Interfaces;

public interface IPostService
{
    Post GetPostById(int id);
    List<Post> GetAllPosts();
    List<Post> GetUserPosts();
    List<Comment> GetPostComments(int id);
    PostResponseModel ClosePost(int id);
    PostResponseModel CreatePost(Post post);
    PostResponseModel UpdatePost(Post post);
    PostResponseModel DeletePost(int id);
}