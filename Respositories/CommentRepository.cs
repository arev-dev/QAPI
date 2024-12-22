using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QAPI.Models;
using QAPI.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using QAPI.Repositories.Interfaces;

namespace QAPI.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly string _connectionString;

    public CommentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public CommentResponseModel CreateComment(Comment comment)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SPCreateComment", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@UserId", comment.UserId);
            command.Parameters.AddWithValue("@PostId", comment.PostId);
            command.Parameters.AddWithValue("@Content", comment.Content);
            var outputParam = new SqlParameter("@NewCommentId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);

            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);
            command.ExecuteNonQuery();

            //obtener valores del SP
            int newPostId = (int)outputParam.Value;
            int returnCode = (int)returnValue.Value;

            Comment commentResponse = new Comment();
            if(newPostId > 0)
            {
                commentResponse = GetCommentById(newPostId);
            }

            return new CommentResponseModel()
            {
                ResponseCode = returnCode, 
                Comment = commentResponse
            };
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error creating comment: {e.Message}");
            return new CommentResponseModel()
            {
                ResponseCode = -99,
                Comment = null
            };
        }
    }

    public CommentResponseModel DeleteComment(int id)
    {
        throw new NotImplementedException();
    }

    public List<Comment> GetAllComments()
    {
        throw new NotImplementedException();
    }

    public Comment GetCommentById(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SPGetCommentById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Comment
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    PostId = (int)reader["PostId"],
                    Content = reader["Content"].ToString() ?? "",
                    CreatedAt = (DateTime)reader["CreatedAt"]
                };
            }
            return new Comment();
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return new Comment();
        }
    }

    public CommentResponseModel UpdateComment(Comment comment)
    {
        throw new NotImplementedException();
    }
}