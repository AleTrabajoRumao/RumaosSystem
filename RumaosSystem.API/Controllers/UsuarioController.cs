using Microsoft.AspNetCore.Mvc;
using RumaosSystem.Application.Interface;
using RumaosSystem.Domain.Entities;

namespace RumaosSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // Constructor que recibe el repositorio de usuarios
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return Ok(usuarios);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        //// Obtener un usuario por su nombre
        //[HttpGet("user/{user}")]
        //public async Task<ActionResult<Usuario>> GetUsuarioByNombre(string user)
        //{
        //    var usuario = await _usuarioRepository.GetByNombreAsync(user);
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(usuario);
        //}

        // Crear un nuevo usuario
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> CrearOValidarUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.User) || string.IsNullOrEmpty(usuario.Password))
            {
                return BadRequest("Usuario y contraseña son requeridos.");
            }

            try
            {
                // Primero intentamos obtener el usuario por su nombre de usuario.
                var usuarioExistente = await _usuarioRepository.GetByNombreAsync(usuario.User);

                if (usuarioExistente != null)
                {
                    // Si el usuario ya existe, devolvemos sus datos.
                    return Ok(usuarioExistente); // El usuario ya existe y no necesitamos crear uno nuevo.
                }

                // Si el usuario no existe, lo creamos.
                await _usuarioRepository.CreateAsync(usuario);

                // Devuelves el usuario recién creado.
                return CreatedAtAction(nameof(CrearOValidarUsuario), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", details = ex.Message });
            }
        }



        // Eliminar un usuario por ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarUsuario(int id)
        {
            try
            {
                var usuarioExistente = await _usuarioRepository.GetByIdAsync(id);

                if (usuarioExistente == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                await _usuarioRepository.DeleteAsync(id);

                return NoContent(); // Devuelve 204 No Content cuando la eliminación es exitosa
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", details = ex.Message });
            }
        }

    }
}
