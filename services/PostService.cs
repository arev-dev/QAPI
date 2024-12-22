using QAPI.Models;
using QAPI.Models.DTO;
using QAPI.Services.Interfaces;

namespace QAPI.Services;
public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public PostResponseModel ClosePost(int id)
    {
        return _postRepository.ClosePost(id);
    }

    public PostResponseModel CreatePost(Post post)
    {
        return _postRepository.CreatePost(post);
    }

    public PostResponseModel DeletePost(int id)
    {
        return _postRepository.DeletePost(id);
    }

    public List<Post> GetAllPosts()
    {
        return _postRepository.GetAllPosts();
    }

    public Post GetPostById(int id)
    {
        return _postRepository.GetPostById(id);
    }

    public List<Comment> GetPostComments(int id)
    {
        return _postRepository.GetPostComments(id);
    }

    public List<UserPostsResponseModel> GetUserPosts(int id)
    {
        return _postRepository.GetUserPosts(id);
    }

    public PostResponseModel UpdatePost(Post post)
    {
        return _postRepository.UpdatePost(post);
    }
}