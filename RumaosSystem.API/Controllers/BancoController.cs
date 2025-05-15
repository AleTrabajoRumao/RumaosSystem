using Microsoft.AspNetCore.Mvc;
using RumaosSystem.Application.Interfaces;
using RumaosSystem.Domain.Entities;
using RumaosSystem.Infrastructure.Repositories;

namespace RumaosSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BancoController : ControllerBase
    {
        private readonly IBancoRepository _bancoRepository;

        public BancoController(IBancoRepository bancoRepository)
        {
            _bancoRepository = bancoRepository;

        }

        // GET: api/banco
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Banco>>> GetAll()
        {
            var bancos = await _bancoRepository.GetAllAsync();
            return Ok(bancos);
        }

        // GET: api/banco/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Banco>> GetById(int id)
        {
            var banco = await _bancoRepository.GetByIdAsync(id);
            if (banco == null)
                return NotFound();

            return Ok(banco);
        }


        // POST: api/banco
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Banco banco)
        {
            if (banco == null)
                return BadRequest("El banco no puede ser nulo.");

            await _bancoRepository.CreateAsync(banco);
            return CreatedAtAction(nameof(GetById), new { id = banco.Id }, banco);
        }

        // PUT: api/banco/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Banco banco)
        {
            if (id != banco.Id)
                return BadRequest("El ID del banco no coincide con el ID de la URL.");

            var bancoExistente = await _bancoRepository.GetByIdAsync(id);
            if (bancoExistente == null)
                return NotFound();

            await _bancoRepository.UpddateAsync(banco);
            return NoContent();
        }

        // DELETE: api/banco/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var banco = await _bancoRepository.GetByIdAsync(id);
            if (banco == null)
                return NotFound();

            await _bancoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
