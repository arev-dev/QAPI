using QAPI.Models;
using QAPI.Models.DTO;

namespace QAPI.Repositories.Interfaces;

public interface ICommentRepository
{
    Comment GetCommentById(int id);
    List<Comment> GetAllComments();
    CommentResponseModel CreateComment(Comment comment);
    CommentResponseModel UpdateComment(Comment comment);
    CommentResponseModel DeleteComment(int id);
}