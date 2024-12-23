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
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand("SPClosePost", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);
            command.ExecuteNonQuery();
            Post postResponse = GetPostById(id);
            int returnCode = (int)returnValue.Value;
            return new PostResponseModel()
            {
                ResponseCode = returnCode,
                Post = postResponse ?? null
            };
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return new PostResponseModel()
            {
                ResponseCode = -99,
                Post = null
            };
        }
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
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand("SPDeletePost", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);
            command.ExecuteNonQuery();
            int returnCode = (int)returnValue.Value;
            Console.WriteLine(returnCode);
            return new PostResponseModel()
            {
                ResponseCode = returnCode,
                Post = null
            };
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return new PostResponseModel()
            {
                ResponseCode = -99,
                Post = null
            };
        }
    }

    public List<Post> GetAllPosts()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand("SPGetAllPosts", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        using var reader = command.ExecuteReader();
        var posts = new List<Post>();
        while (reader.Read())
        {
            posts.Add(new Post
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                Title = reader["Title"].ToString() ?? "",
                Content = reader["Content"].ToString() ?? "",
                IsClosed = (bool)reader["IsClosed"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }
        return posts;
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

    public List<UserCommentsPostResponseModel> GetPostComments(int id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand("SPGetPostComments", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            var comments = new List<UserCommentsPostResponseModel>();
            while (reader.Read())
            {
                comments.Add(new UserCommentsPostResponseModel
                {
                    CommentId = (int)reader["CommentId"],
                    PostId = (int)reader["PostId"],
                    UserId = (int)reader["UserId"],
                    Content = reader["Content"].ToString() ?? "",
                    Username = reader["Username"].ToString()?? "",
                    CreatedAt = (DateTime)reader["CreatedAt"]
                });
            }
            return comments;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return new List<UserCommentsPostResponseModel>();
        }
    }

    public List<UserPostsResponseModel> GetUserPosts(int Id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand("SPGetUserPosts", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@UserId", Id);
    
        using var reader = command.ExecuteReader();
        var posts = new List<UserPostsResponseModel>();
     
        
        while (reader.Read())
        {
            posts.Add(new UserPostsResponseModel
            {
                PostId = (int)reader["PostId"],
                UserId = (int)reader["UserId"],
                Username = reader["Username"].ToString() ?? "",
                Title = reader["Title"].ToString() ?? "",
                Content = reader["Content"].ToString() ?? "",
                IsClosed = (bool)reader["IsClosed"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }

        return posts;
    }

    public PostResponseModel UpdatePost(Post post)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand("SPUpdatePost", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Id", post.Id);
            command.Parameters.AddWithValue("@Title", post.Title);
            command.Parameters.AddWithValue("@Content", post.Content);
            command.Parameters.AddWithValue("@IsClosed", post.IsClosed);
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);
            command.ExecuteNonQuery();
            int returnCode = (int)returnValue.Value;
            Post postResponse = new Post();
            if(returnCode == 1)
            {
                postResponse = GetPostById(post.Id);
            }
            return new PostResponseModel()
            {
                ResponseCode = returnCode,
                Post = postResponse
            };
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return new PostResponseModel()
            {
                ResponseCode = -99,
                Post = null
            };
        }
    }
}