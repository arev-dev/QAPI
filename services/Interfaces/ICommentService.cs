using QAPI.Models;
using QAPI.Models.DTO;

namespace QAPI.Services.Interfaces;

public interface ICommentService
{
    Comment GetCommentById(int id);
    List<Comment> GetAllComments();
    CommentResponseModel CreateComment(Comment comment);
    CommentResponseModel UpdateComment(Comment comment);
    CommentResponseModel DeleteComment(int id);
}