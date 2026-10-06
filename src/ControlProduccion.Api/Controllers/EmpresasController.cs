using System.Threading.Tasks;
using ControlProduccion.Compac.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ControlProduccion.Api.Controllers
{
    public class AbrirEmpresaRequest
    {
        /// <summary>Directorio de la empresa. Si se omite se usa Compac:DirectorioEmpresa.</summary>
        public string Directorio { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly SesionCompac _sesion;

        public EmpresasController(SesionCompac sesion) => _sesion = sesion;

        [HttpPost("abrir")]
        public async Task<IActionResult> Abrir([FromBody] AbrirEmpresaRequest request)
        {
            var directorio = await _sesion.AbrirEmpresaAsync(request?.Directorio);
            return Ok(new { mensaje = "Empresa abierta.", directorio });
        }
    }
}
