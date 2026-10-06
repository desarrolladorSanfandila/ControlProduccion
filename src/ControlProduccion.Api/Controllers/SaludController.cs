using Microsoft.AspNetCore.Mvc;

namespace ControlProduccion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaludController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { estado = "Servicio de api arriba" });
    }
}
