using CrediAPI.Domain.Models;
using System.Threading.Tasks;

namespace CrediAPI.Domain.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioModel> FindByUsername(string username);
    }
}