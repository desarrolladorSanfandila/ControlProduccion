using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ControlProduccion.Compac.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ControlProduccion.Api.Controllers
{
    public class LoginRequest
    {
        [Required] public string Usuario { get; set; }
        [Required] public string Contrasenia { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SesionController : ControllerBase
    {
        private readonly SesionCompac _sesion;

        public SesionController(SesionCompac sesion) => _sesion = sesion;

        /// <summary>Inicia sesión en el SDK de Compac con el usuario de CONTPAQi Comercial.</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            await _sesion.IniciarSesionAsync(request.Usuario, request.Contrasenia);
            return Ok(new { mensaje = "Sesión iniciada en el SDK de Compac." });
        }
    }
}
