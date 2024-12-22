using QAPI.Models;
using QAPI.Models.DTO;
using QAPI.Repositories.Interfaces;
using QAPI.Services.Interfaces;

namespace QAPI.Services;
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public CommentResponseModel CreateComment(Comment comment)
    {
        return _commentRepository.CreateComment(comment);
    }

    public CommentResponseModel DeleteComment(int id)
    {
        return _commentRepository.DeleteComment(id);
    }

    public List<Comment> GetAllComments()
    {
        return _commentRepository.GetAllComments();
    }

    public Comment GetCommentById(int id)
    {
        return _commentRepository.GetCommentById(id);
    }

    public CommentResponseModel UpdateComment(Comment comment)
    {
        return _commentRepository.UpdateComment(comment);
    }
}