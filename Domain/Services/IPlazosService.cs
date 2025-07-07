using CrediAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrediAPI.Domain.Services
{
    public interface IPlazosService
    {
        Task<List<PlazosModel>> FindAll();
        Task<PlazosModel> FindById(int id);
    }
}