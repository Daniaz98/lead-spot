using System.Data;
using LeadSpot.Models;
using Microsoft.Data.SqlClient;

namespace LeadSpot.Data;

public class DataAccess
{    
     //conexão ao banco
     private SqlConnection _connection = null!;
     //comandos de requisição
     private SqlCommand _command = null!;
     //Auxilia na conexão utilizando a conexão string em appsettings
     public static IConfiguration Configuration { get; set; }

     private string GetConnectionString()
     {
          var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
          
          Configuration = builder.Build();
          
          return Configuration.GetConnectionString("DefaultConnection");
     }

     public List<User> GetUsers()
     {
          List<User> users = new List<User>();
          
          using (_connection = new SqlConnection(GetConnectionString()))
          {
               _command = _connection.CreateCommand();
               _command.CommandType = CommandType.StoredProcedure;
               _command.CommandText = "[DBO].[listar_usuarios]";
               
               _connection.Open();
               
               SqlDataReader reader = _command.ExecuteReader();

               while (reader.Read())
               {
                    User user = new User();
                    
                    user.Id = (int)reader["Id"];
                    user.Name = (string)reader["Name"];
                    user.Email = (string)reader["Email"];
                    user.Number = (string)reader["Number"];
                    
                    users.Add(user);
                    
               }
               _connection.Close();
          }
          return users;
     }
}