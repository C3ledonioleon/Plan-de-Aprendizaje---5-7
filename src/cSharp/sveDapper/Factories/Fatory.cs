using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Security.Claims;


namespace sveDapper.Factories;
public interface IDbConnectionFactory
{
   IDbConnection CreateConnection();
}
public class RoleBasedDbConnectionFactory : IDbConnectionFactory
{
   private readonly IHttpContextAccessor _httpContextAccessor;
   private readonly IConfiguration _configuration;


   public RoleBasedDbConnectionFactory(IHttpContextAccessor httpContextAccessor,
                                       IConfiguration configuration)
   {
       _httpContextAccessor = httpContextAccessor;
       _configuration = configuration;
   }


   public IDbConnection CreateConnection()
   {
      
       var user = _httpContextAccessor.HttpContext?.User;
       if (user == null || !user.Identity!.IsAuthenticated)
           throw new UnauthorizedAccessException("Usuario no autenticado.");


       // Se obtiene el rol del JWT
       var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;


       if (string.IsNullOrEmpty(role))
           throw new Exception("No se pudo determinar el rol del usuario.");


       // Se busca la ConnectionString con el mismo nombre del rol
       var connString = _configuration.GetConnectionString(role);


       if (string.IsNullOrEmpty(connString))
           throw new Exception($"No existe ConnectionString configurada para el rol '{role}'.");


       return new MySqlConnection(connString);
   }
}
