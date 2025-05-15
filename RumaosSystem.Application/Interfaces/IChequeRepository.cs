using RumaosSystem.Application.Dtos;
using RumaosSystem.Domain.Entities;

namespace RumaosSystem.Application.Interfaces
{
    public interface IChequeRepository
    {
        Task<IEnumerable<Cheque>> GetAllAsync();
        Task<Cheque?> GetByIdAsync(int id);
        Task UpdateAsync(Cheque cheque);
        Task<IEnumerable<Cheque>> GetChequesDisponiblesAsync();
        Task<bool> ActualizarChequeAsync(ChequeDto chequeDto);


        //Task<Cheque?> GetByCompositeKeyAsync(int id, string uen, int nroRecibo, int ptovtaRec);

        Task<List<(int NroCheque, DateTime FechaVto, int NroRecibo)>> GetChequesDestinoExistentesAsync();
        Task InsertarChequesDestinoAsync(List<ChequeDestino> cheques);
        Task InsertarCheques(List<Cheque> cheques);

            Task<int?> GetBancoIdByNombreAsync(string bancoNombre);




     
       
       












    }
}
