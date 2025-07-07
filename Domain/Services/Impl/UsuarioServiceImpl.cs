using CrediAPI.Domain.Models;
using CrediAPI.Infrastructure.Context;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CrediAPI.Domain.Services.Impl
{
    public class UsuarioServiceImpl : IUsuarioService
    {
        private readonly CrediDBContext _context;

        public UsuarioServiceImpl(CrediDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UsuarioModel> FindByUsername(string username)
        {
            try
            {
                var user = await _context.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                return user;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the user with username {username}.", ex);
            }
        }
    }
}