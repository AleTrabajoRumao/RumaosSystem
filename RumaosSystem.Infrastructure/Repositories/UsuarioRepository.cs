using Microsoft.EntityFrameworkCore;
using RumaosSystem.Application.Interface;
using RumaosSystem.Domain.Entities;
using RumaosSystem.Infrastructure.Persistence;

namespace RumaosSystem.Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
        
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        
        public async Task<Usuario?> GetByNombreAsync(string user)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.User == user);
        }
        
        public async Task CreateAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Buscar el usuario por el ID
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario); // Eliminar el usuario
                await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
            }
            else
            {
                throw new Exception("Usuario no encontrado");
            }
        }

    }
}
