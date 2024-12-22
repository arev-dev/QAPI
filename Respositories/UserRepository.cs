using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QAPI.Models;

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
        var command = new SqlCommand("GetUserById", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
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

    public int CreateUser(User user)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand("CreateUser", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            // Agregar parámetros al procedimiento almacenado
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);

            // Ejecutar el procedimiento almacenado y obtener el código de retorno
            var returnValue = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnValue);

            command.ExecuteNonQuery();

            // Retornar el código de retorno
            return (int)returnValue.Value;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error SQL: {ex.Message}");
            return -99; // Código genérico de error de SQL
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return -99; // Código genérico de error inesperado
        }
    }

}
