using CrediAPI.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrediAPI.Domain.Services
{
    public interface IProductoService
    {
        Task<List<ProductoModel>> FindAll();
        Task<ProductoModel> FindById(int id);
    }
}