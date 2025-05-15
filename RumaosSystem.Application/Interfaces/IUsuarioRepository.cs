using RumaosSystem.Domain.Entities;

namespace RumaosSystem.Application.Interface
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);

        Task<Usuario?> GetByNombreAsync(string user);

        Task CreateAsync(Usuario usuario);

        Task DeleteAsync(int id);

    }
}
