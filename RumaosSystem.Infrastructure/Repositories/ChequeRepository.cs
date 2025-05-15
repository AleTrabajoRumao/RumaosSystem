using System.Linq;
using System.Numerics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RumaosSystem.Application.Dtos;
using RumaosSystem.Application.Interfaces;
using RumaosSystem.Domain.Entities;
using RumaosSystem.Infrastructure.Persistence;

namespace RumaosSystem.Infrastructure.Repositories
{
    public class ChequeRepository : IChequeRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;


        public ChequeRepository(AppDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Asigna el logger

        }

        // GET con filtros
        public async Task<IEnumerable<Cheque>> GetAllAsync()
        {
            var cheques = await _context.Cheques
                .Where(c => c.Mediopago == 4 && c.Banco!= "ECHEQ")
                .OrderByDescending(c => c.Id)
                .Take(1000)
                .ToListAsync(); // Trae los datos de la base

            return cheques.Where(c =>
                DateTime.TryParse(c.Fechavtosql.ToString(), out var fecha) &&
                fecha >= new DateTime(2025, 3, 1));
        }


        public async Task<Cheque> GetByIdAsync(int id)
        {
            return await _context.Cheques.FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task UpdateAsync(Cheque cheque)
        {
            _context.Cheques.Update(cheque);
            await _context.SaveChangesAsync();
        }

      


        public async Task<int?> GetBancoIdByNombreAsync(string bancoNombre)
        {
            var idBanco = await (
                from banco in _context.Bancos
                where banco.Nombre == bancoNombre // Coincidir por el nombre del banco
                select (int?)banco.Id
            ).FirstOrDefaultAsync(); // Devuelve null si no encuentra ningún banco

            if (idBanco == null)
            {
                throw new Exception($"El banco '{bancoNombre}' no fue encontrado en la tabla Banco.");
            }

            return idBanco;
        }




        public async Task<IEnumerable<Cheque>> GetChequesDisponiblesAsync()
        {
            var cheques = new List<Cheque>();

            // 1. Traer todos los nombres de bancos válidos
            var bancosValidos = await _context.Bancos
                .Select(b => b.Nombre.Trim().ToUpper())
                .ToListAsync();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            SELECT c.Id, c.UEN, c.PTOVTAREC AS ptovta, c.NRORECIBO, c.BANCO, 
                   c.FECHAVTOSQL AS FECHAVTO, c.NROCHEQUE, c.IMPORTE, fechasql AS fechaingreso
            FROM CCRec02 AS c 
            LEFT JOIN RecVenta AS r 
                ON c.UEN = r.UEN 
               AND c.PTOVTAREC = r.PTOVTA 
               AND c.NRORECIBO = r.NRORECIBO
            LEFT JOIN cheques d 
                ON c.NROCHEQUE = d.NROCHEQUE 
               AND c.FECHAVTOSQL = d.FECHAVTO 
               AND c.NRORECIBO = d.NRORECIBO
            WHERE d.NROCHEQUE IS NULL
              AND c.FECHAVTOSQL >= '2025-03-01'
              AND c.MEDIOPAGO = 4
              AND c.BANCO <> 'ECHEQ'";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Banco leído desde CCRec02
                        var bancoLeido = reader["BANCO"].ToString()?.Trim().ToUpper();

                        // Verificamos si coincide con algún banco válido
                        var bancoFinal = bancosValidos.Contains(bancoLeido) ? bancoLeido : "ERROR ⚠️";

                        cheques.Add(new Cheque
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Uen = reader["UEN"].ToString(),
                            Ptovtarec = Convert.ToInt32(reader["PTOVTA"]),
                            Nrorecibo = Convert.ToInt32(reader["NRORECIBO"]),
                            Banco = bancoFinal, // Reemplazamos con el banco validado
                            Fechavtosql = Convert.ToDateTime(reader["FECHAVTO"]),
                            Nrocheque = Convert.ToInt32(reader["NROCHEQUE"]),
                            Importe = Convert.ToDecimal(reader["IMPORTE"]),
                            FechaIngreso = reader["fechaingreso"] != DBNull.Value
                                ? Convert.ToDateTime(reader["fechaingreso"])
                                : default(DateTime)
                        });
                    }
                }
            }

            return cheques;
        }


        

        public async Task<int?> ObtenerIdBancoAsync(string nombreBanco)
        {
            var banco = await _context.Bancos
                                      .Where(b => b.Nombre == nombreBanco)
                                      .FirstOrDefaultAsync();

            return banco?.Id; // Retorna el IdBanco o null si no se encuentra
        }


        public async Task<bool> ActualizarChequeAsync(ChequeDto chequeDto)
        {
            var cheque = await _context.Cheques.FirstOrDefaultAsync(c =>
                c.Id == chequeDto.Id &&
                c.Uen == chequeDto.Uen &&
                c.Nrorecibo == chequeDto.Nrorecibo &&
                c.Ptovtarec == chequeDto.Ptovtarec);

            if (cheque == null)
                return false;

            // Actualizá los campos deseados (ejemplo: banco, nrocheque, fechavto)
            cheque.Banco = chequeDto.Banco;
            cheque.Nrocheque = chequeDto.Nrocheque;
            cheque.Fechavtosql = chequeDto.Fechavtosql;

            await _context.SaveChangesAsync();
            return true;
        }




        public async Task<List<(int NroCheque, DateTime FechaVto, int NroRecibo)>> GetChequesDestinoExistentesAsync()
        {
            return await _context.ChequesDestino
                .Select(c => new ValueTuple<int, DateTime, int>(
                    c.Nrocheque ?? 0,
                    c.Fechavto ?? DateTime.MinValue,
                    c.Nrorecibo ?? 0
                ))
                .ToListAsync();
        }




        public async Task InsertarChequesDestinoAsync(List<ChequeDestino> cheques)
        {
            var existentes = await GetChequesDestinoExistentesAsync();

            var chequesFiltrados = cheques
                .Where(c => !existentes.Any(e =>
                    e.NroCheque == c.Nrocheque &&
                    e.FechaVto.Date == c.Fechavto.Value.Date &&
                    e.NroRecibo == c.Nrorecibo))
                .ToList();

            if (chequesFiltrados.Any())
            {
                await _context.ChequesDestino.AddRangeAsync(chequesFiltrados);
                await _context.SaveChangesAsync();
            }
        }


        public async Task InsertarCheques(List<Cheque> cheques)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var fechaMinima = new DateTime(2000, 1, 1);
                var fechaHoy = DateTime.Now;

                // 1. Validar cheques ya existentes en la tabla de destino
                var chequesDestino = cheques.Select(c => new ChequeDestino
                {
                    Nrocheque = (int?)c.Nrocheque,
                    Fechavto = c.Fechavtosql,
                    Nrorecibo = (int?)c.Nrorecibo,
                    Importe = (double?)c.Importe,
                    Banco = c.Banco,
                    Uen = c.Uen,
                    Ptovta = (int?)c.Ptovtarec
                }).ToList();

                await InsertarChequesDestinoAsync(chequesDestino);  // Filtra los cheques ya insertados

                //-----------------------------------------------------------------------------------------------------------




                var chequesMapeadosASgfinCartera = new List<SgfinCartera>();

                foreach (var cc in cheques)
                {
                    // Buscar el IdBanco correspondiente al nombre del banco
                    var idBanco = await (
                        from banco in _context.Bancos
                        where banco.Nombre == cc.Banco // Matchear por nombre del banco
                        select (int?)banco.Id
                    ).FirstOrDefaultAsync(); // devuelve null si no encuentra

                    if (idBanco == null)
                    {
                        throw new Exception($"El banco '{cc.Banco}' no fue encontrado en la tabla Banco.");
                    }


                    foreach (var c in cheques)
                    {
                        // Buscar el IdProveedorIngreso correspondiente
                        var idProveedorIngreso = await (
                            from rec in _context.RecVenta
                            join prov in _context.SgfinPlanProveedor2
                            on rec.Nrocliente.ToString() equals prov.Cuit
                            where rec.Uen == c.Uen &&
                                  rec.Ptovta == c.Ptovtarec &&
                                  rec.Nrorecibo == c.Nrorecibo
                            select prov.Id
                        ).FirstOrDefaultAsync(); // devuelve null si no encuentra

                        var cheque = new SgfinCartera
                        {
                            IdBanco = idBanco,
                            Numero = c.Nrocheque.ToString(),
                            Importe = (double)c.Importe,
                            FechaEmision = fechaHoy,
                            FechaIngreso = fechaHoy,
                            IdProveedorIngreso = idProveedorIngreso,
                            FechaCobro = c.Fechavtosql,
                            Caja = 42,
                            Estado = "1",
                        };

                        chequesMapeadosASgfinCartera.Add(cheque);
                    }

                    // Guardar en la base de datos
                    await _context.SgfinCartera.AddRangeAsync(chequesMapeadosASgfinCartera);
                    await _context.SaveChangesAsync();






                    //-----------------------------------------------------------------------------------------------------------
                var idArqueo = await _context.SgfinArqueo
                    .Where(a => a.IdBox == 42 && a.Fecha.Date == fechaHoy.Date)

                    .OrderByDescending(a => a.Fecha)
                    .Select(a => a.Id)
                    .FirstOrDefaultAsync();

                if (idArqueo == 0)
                {
                    throw new Exception("No se encontró un Id válido en la tabla SgfinArqueo.");
                }




                    // 3. Mapeo de SGFIN_IngresoCaja_Nueva

                    var chequesMapeadosASgfinIngresoCaja = chequesMapeadosASgfinCartera.Select(c => new SgfinIngresoCaja
                    {
                        IdArqueo = idArqueo,
                        Fecha = fechaHoy,
                        Descripcion = null,
                        Importe = c.Importe, // Usa el importe de SgfinCartera
                        IdDepartamento = 14,
                        IdReferencia = 63,
                        IdRendicion = null,
                        IdProveedor = c.IdProveedorIngreso, // Asignar el mismo IdProveedorIngreso
                        CentroCosto = 4,
                        Activo = null,
                    }).ToList();

                    await _context.SgfinIngresoCaja.AddRangeAsync(chequesMapeadosASgfinIngresoCaja);
                    await _context.SaveChangesAsync();
                    //-----------------------------------------------------------------------------------------------------------
                    // 4. Mapeo de SGFIN_DetalleArqueoCartera_Nueva


                    var chequesMapeadosASgfinDetalleArqueoCartera = chequesMapeadosASgfinCartera.Select(c => new SgfinDetalleArqueoCartera
                    {
                        IdArqueo = idArqueo,
                        IdCartera = c.Id
                    }).ToList();

                    await _context.SgfinDetalleArqueoCartera.AddRangeAsync(chequesMapeadosASgfinDetalleArqueoCartera);
                    await _context.SaveChangesAsync();


                    //-----------------------------------------------------------------------------------------------------------



                    // 6. Mapeo de SGFIN_DetalleIngreso
                    var chequesMapeadosASgfinDetalleIngreso = new List<SgfinDetalleIngreso>();

                    // Necesitamos emparejar los registros de SGFIN_IngresoCaja con los de SGFIN_Cartera
                    for (int i = 0; i < chequesMapeadosASgfinIngresoCaja.Count; i++)
                    {
                        if (i < chequesMapeadosASgfinCartera.Count)
                        {
                            var detalleIngreso = new SgfinDetalleIngreso
                            {
                                IdIngreso = (int)chequesMapeadosASgfinIngresoCaja[i].Id, // ID creado en SGFIN_IngresoCaja
                                Importe = (int?)chequesMapeadosASgfinCartera[i].Importe,  // Importe del cheque
                                IdCartera = chequesMapeadosASgfinCartera[i].Id      // ID creado en SGFIN_Cartera
                            };

                            chequesMapeadosASgfinDetalleIngreso.Add(detalleIngreso);
                        }
                    }

                    await _context.SgfinDetalleIngreso.AddRangeAsync(chequesMapeadosASgfinDetalleIngreso);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync(); // Confirmar la transacción si todo ha sido exitoso

                }
            }

            //--------------------------------------------------------------------------------------------------------------------
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // 🔁 Reversar todo si algo falla

                Console.WriteLine("Error al insertar los cheques: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);

                throw;
            }
        }

    }
}


















