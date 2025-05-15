using Microsoft.AspNetCore.Mvc;
using RumaosSystem.Application.Interfaces;
using RumaosSystem.Domain.Entities;
using RumaosSystem.Application.Dtos;
using RumaosSystem.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;


namespace RumaosSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChequeController : ControllerBase
    {
        private readonly IChequeRepository _chequeRepository;

        public ChequeController(IChequeRepository chequeRepository)
        {
            _chequeRepository = chequeRepository;

        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cheque>>> Get()
        {
            var cheques = await _chequeRepository.GetAllAsync();
            return Ok(cheques);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cheque = await _chequeRepository.GetByIdAsync(id);
            if (cheque == null)
                return NotFound();

            return Ok(cheque);


        }


        
        [HttpGet("GetBancoIdByNombre/{bancoNombre}")]
        public async Task<IActionResult> GetBancoIdByNombre(string bancoNombre)
        {
            try
            {
                var idBanco = await _chequeRepository.GetBancoIdByNombreAsync(bancoNombre);
                return Ok(idBanco);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<Cheque>>> GetChequesDisponibles()
        {
            var cheques = await _chequeRepository.GetChequesDisponiblesAsync();
            return Ok(cheques);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCheque(int id, [FromBody] ChequeDto chequeDto)
        {
            var cheque = await _chequeRepository.GetByIdAsync(id);
            if (cheque == null)
                return NotFound();

            cheque.Banco = chequeDto.Banco;
            cheque.Nrocheque = chequeDto.Nrocheque;
            cheque.Fechavtosql = chequeDto.Fechavtosql;

            await _chequeRepository.UpdateAsync(cheque);
            return Ok(cheque);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarCheque([FromBody] ChequeDto chequeDto)
        {
            if (chequeDto == null)
                return BadRequest("Datos inválidos.");

            var resultado = await _chequeRepository.ActualizarChequeAsync(chequeDto);

            if (!resultado)
                return NotFound(new { success = false, message = "Cheque no encontrado con los datos proporcionados." });

            return Ok(new { success = true, message = "Cheque actualizado correctamente." });
        }





        [HttpPost("insertar")]
        public async Task<IActionResult> InsertarCheques([FromBody] List<Cheque> cheques)
        {
            if (cheques == null || !cheques.Any())
                return BadRequest("La lista de cheques está vacía.");

            try
            {
                await _chequeRepository.InsertarCheques(cheques);
                return Ok(new { mensaje = "Cheques insertados correctamente." });

            }
            catch (Exception ex)
            {
                // Puedes loguear el error aquí si lo necesitas
                return StatusCode(500, $"Error al insertar los cheques: {ex.Message}");
            }
        }
}



       










    }


