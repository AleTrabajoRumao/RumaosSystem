using Microsoft.EntityFrameworkCore;
using RumaosSystem.Application.Dtos;
using RumaosSystem.Domain.Entities;

namespace RumaosSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cheque> Cheques { get; set; }
        public DbSet<ChequeDestino> ChequesDestino { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SgfinArqueo> SgfinArqueo { get; set; }
        public DbSet<SgfinCartera> SgfinCartera { get; set; }
        public DbSet<SgfinIngresoCaja> SgfinIngresoCaja { get; set; }
        public DbSet<SgfinDetalleArqueoCartera> SgfinDetalleArqueoCartera { get; set; }
        public DbSet<SgfinDetalleIngreso> SgfinDetalleIngreso { get; set; }
        public DbSet<RecVenta> RecVenta { get; set; }
        public DbSet<SgfinPlanProveedor2> SgfinPlanProveedor2 { get; set; }
        public DbSet<Banco> Bancos { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración opcional: nombre de tabla, tipos, etc.

            modelBuilder.Entity<Cheque>()
        .ToTable("CCRec02")
        .HasKey(c => new { c.Id, c.Uen, c.Nrorecibo, c.Ptovtarec });
            modelBuilder.Entity<ChequeDestino>().ToTable("cheques"); 
            modelBuilder.Entity<SgfinArqueo>().ToTable("SGFIN_Arqueo");
            modelBuilder.Entity<SgfinCartera>().ToTable("SGFIN_Cartera_Nueva");
            modelBuilder.Entity<SgfinIngresoCaja>().ToTable("SGFIN_IngresoCaja_Nueva");
            modelBuilder.Entity<SgfinDetalleArqueoCartera>().ToTable("SGFIN_DetalleArqueoCartera_Nueva");
            modelBuilder.Entity<SgfinDetalleIngreso>().ToTable("SGFIN_DetalleIngreso_Nueva");
            modelBuilder.Entity<RecVenta>().ToTable("RecVenta");
            modelBuilder.Entity<SgfinPlanProveedor2>().ToTable("SGFIN_PlanProveedor2");
            modelBuilder.Entity<Banco>().ToTable("SGFIN_Banco");






            base.OnModelCreating(modelBuilder);
            
        }
    }
}
