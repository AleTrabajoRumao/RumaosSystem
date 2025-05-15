using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RumaosSystem.Application.Interfaces;
using RumaosSystem.Domain.Entities;
using RumaosSystem.Infrastructure.Persistence;

namespace RumaosSystem.Infrastructure.Repositories
{
    public class BancoRepository : IBancoRepository
    {
        private readonly AppDbContext _context;

        public BancoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Banco>> GetAllAsync()
        {
            return await _context.Set<Banco>().ToListAsync();
        }

        public async Task<Banco?> GetByIdAsync(int id)
        {
            return await _context.Set<Banco>().FindAsync(id);
        }

        public async Task CreateAsync(Banco banco)
        {
            await _context.Set<Banco>().AddAsync(banco);
            await _context.SaveChangesAsync();
        }

        public async Task UpddateAsync(Banco banco)
        {
            _context.Set<Banco>().Update(banco);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var banco = await GetByIdAsync(id);
            if (banco != null)
            {
                _context.Set<Banco>().Remove(banco);
                await _context.SaveChangesAsync();
            }
        }
    }
}
