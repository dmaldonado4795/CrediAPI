using CrediAPI.Domain.Models;
using CrediAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CrediAPI.Domain.Services.Impl
{
    public class PlazosServiceImpl : IPlazosService
    {
        private readonly CrediDBContext _context;

        public PlazosServiceImpl(CrediDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<PlazosModel>> FindAll()
        {
            try
            {
                var plazos = await _context.Plazos
                    .AsNoTracking()
                    .ToListAsync();
                return plazos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all plazos.", ex);
            }
        }

        public async Task<PlazosModel> FindById(int id)
        {
            try
            {
                var plazo = await _context.Plazos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
                return plazo;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the plazo with ID {id}.", ex);
            }
        }
    }
}