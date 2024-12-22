using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QAPI.Models;
using QAPI.Models.DTO;

namespace QAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public User GetUserById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand("SPGetUserById", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Id = (int)reader["Id"],
                Username = reader["Username"].ToString() ?? "",
                Password = reader["Password"].ToString() ?? "",
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
        return new User();
    }

    public UserResponseModel CreateUser(User user)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("SPCreateUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
             // Hashear la contraseña
            string hashedPassword = Hassher.HashPassword(user.Password);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);

            var outputParam = new SqlParameter("@NewUserId", SqlDbType.Int)
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
            int newUserId = (int)outputParam.Value;
            int returnCode = (int)returnValue.Value;
            User userResponse = new User();

            if(newUserId > 0)
            {
                userResponse = GetUserById(newUserId);
            }

            return new UserResponseModel()
            {
                ResponseCode = returnCode, 
                User = userResponse
            };
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error SQL: {ex.Message}");
            return new UserResponseModel()
            {
                ResponseCode = -99,
                User = null 
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new UserResponseModel()
            {
                ResponseCode = -99,
                User = null
            };
        }
    }

    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand("SPGetAllUsers", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = (int)reader["Id"],
                Username = reader["Username"].ToString() ?? "",
                Password = reader["Password"].ToString() ?? "",
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }

        return users;
    }

    public UserResponseModel UpdateUser(User user)
    {
        try
        {
            //Abrir conexion
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SPUpdateUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            //Parametros
            string hashedPassword = Hassher.HashPassword(user.Password);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);
            command.ExecuteNonQuery();
            int returnCode = (int)returnValue.Value;
            User userResponse = new User();

            if(returnCode == 1)
            {
                userResponse = GetUserById(user.Id);
            }

            //Retornar respuesta
            return new UserResponseModel()
            {
                ResponseCode = returnCode, 
                User = userResponse
            };
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error SQL: {e.Message}");
            return new UserResponseModel()
            {
                ResponseCode = -99,
                User = null 
            };
        }
    }

    public UserResponseModel DeleteUser(int id)
    {
        try
        {
            //Abrir conexion
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SPDeleteUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            //Parametros
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

            //Retornar respuesta
            return new UserResponseModel()
            {
                ResponseCode = returnCode, 
                User = null
            };

        }
        catch(Exception e)
        {
            Console.WriteLine($"Error SQL: {e.Message}");
            return new UserResponseModel()
            {
                ResponseCode = -99,
                User = null
            };
        }
    }

    public UserResponseModel Login(User user)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SPLoginUser", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            string hashedPassword = Hassher.HashPassword(user.Password);
            //Parametros
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);
            command.ExecuteNonQuery();

            int returnCode = (int)returnValue.Value;
            //Retornar respuesta
            return new UserResponseModel()
            {
                ResponseCode = returnCode, 
                User = null
            };

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error SQL: {e.Message}");
            return new UserResponseModel()
            {
                ResponseCode = -99,
                User = null
            };
        }
    }

    public User GetUserByUsername(string username)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand("SPGetUserByUsername", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Username", username);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = (int)reader["Id"],
                    Username = reader["Username"].ToString() ?? "",
                    Password = reader["Password"].ToString() ?? "",
                    CreatedAt = (DateTime)reader["CreatedAt"]
                };
            }
            return new User();
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error SQL: {e.Message}");
            return new User();
        }
    }
}
