using CrediAPI.Domain.Models;
using CrediAPI.Web.DTOs.Response;

namespace CrediAPI.Domain.Services
{
    public interface IAuthService
    {
        LoginResponse Authenticate(UsuarioModel usuario, string password);
    }
}