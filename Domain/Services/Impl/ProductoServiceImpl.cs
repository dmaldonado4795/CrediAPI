using CrediAPI.Domain.Models;
using CrediAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CrediAPI.Domain.Services.Impl
{
    public class ProductoServiceImpl : IProductoService
    {
        private readonly CrediDBContext _context;

        public ProductoServiceImpl(CrediDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ProductoModel>> FindAll()
        {
            try
            {
                var productos = await _context.Productos
                    .AsNoTracking()
                    .ToListAsync();
                return productos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all products.", ex);
            }
        }

        public async Task<ProductoModel> FindById(int id)
        {
            try
            {
                var producto = await _context.Productos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
                return producto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the product with ID {id}.", ex);
            }
        }
    }
}