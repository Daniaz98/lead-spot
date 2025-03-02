using System.Data;
using LeadSpot.Models;
using Microsoft.Data.SqlClient;

namespace LeadSpot.Data;

public class DataAccess 
{    
     //conexão ao banco
     SqlConnection _connection = null!;
     
     //comandos de requisição
     SqlCommand _command = null!;
     
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
               _command.CommandText = "[DBO].[list_usuarios]";
               
               _connection.Open();
               
               SqlDataReader reader = _command.ExecuteReader();

               while (reader.Read())
               {
                    User user = new User();
                    
                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.Name = reader["Name"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.Number = reader["Number"].ToString();
                    user.Status = reader["Status_lead"].ToString();
                    
                    users.Add(user);
                    
               }
               
               _connection.Close();
          }
          return users;
     }
}