using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RumaosSystem.Domain.Entities;

namespace RumaosSystem.Application.Interfaces
{
    public interface IBancoRepository
    {
        Task<IEnumerable<Banco>> GetAllAsync();
        Task<Banco?> GetByIdAsync(int id);
        Task CreateAsync(Banco banco);
        Task UpddateAsync(Banco banco);
        Task DeleteAsync(int id);
    }
}
