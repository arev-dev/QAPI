using System.Data;
using Microsoft.Data.SqlClient;
using QAPI.Models;
using QAPI.Models.DTO;

namespace QAPI.Repositories;

public class PostRepository : IPostRepository
{
    private readonly string _connectionString;

    public PostRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
    public PostResponseModel ClosePost(int id)
    {
        throw new NotImplementedException();
    }

    public PostResponseModel CreatePost(Post post)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SPCreatePost", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            // Hashear la contraseña
           
            command.Parameters.AddWithValue("@UserId", post.UserId);
            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Content", post.Content);

            var outputParam = new SqlParameter("@NewPostId", SqlDbType.Int)
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
            Post postResponse = new Post();
            if(newPostId > 0)
            {
                postResponse = GetPostById(newPostId);
            }

            return new PostResponseModel()
            {
                ResponseCode = returnCode, 
                Post = postResponse
            };
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error SQL: {ex.Message}");
            return new PostResponseModel()
            {
                ResponseCode = -99,
                Post = null
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new PostResponseModel()
            {
                ResponseCode = -99,
                Post = null
            };
        }
    }

    public PostResponseModel DeletePost(int id)
    {
        throw new NotImplementedException();
    }

    public List<Post> GetAllPosts()
    {
        throw new NotImplementedException();
    }

    public Post GetPostById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand("SPGetPostById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Post
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                Title = reader["Title"].ToString() ?? "",
                Content = reader["Content"].ToString() ?? "",
                IsClosed = (bool)reader["IsClosed"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
        return new Post();
    }

    public List<Comment> GetPostComments(int id)
    {
        throw new NotImplementedException();
    }

    public List<Post> GetUserPosts()
    {
        throw new NotImplementedException();
    }

    public PostResponseModel UpdatePost(Post post)
    {
        throw new NotImplementedException();
    }
}