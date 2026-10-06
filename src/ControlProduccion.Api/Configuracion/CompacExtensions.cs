using ControlProduccion.Compac;
using ControlProduccion.Compac.Servicios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControlProduccion.Api.Configuracion
{
    public static class CompacExtensions
    {
        public static IServiceCollection AddCompac(this IServiceCollection services, IConfiguration config)
        {
            var opciones = config.GetSection("Compac").Get<CompacOptions>() ?? new CompacOptions();
            services.AddSingleton(opciones);
            services.AddSingleton<EjecutorSdk>();
            services.AddSingleton<SesionCompac>();
            return services;
        }

        /// <summary>Convierte los errores del SDK en respuestas 400 con el código y mensaje del SDK.</summary>
        public static IApplicationBuilder UseErroresCompac(this IApplicationBuilder app) =>
            app.Use(async (ctx, next) =>
            {
                try { await next(); }
                catch (CompacException ex)
                {
                    ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await ctx.Response.WriteAsJsonAsync(new
                    {
                        title = "Error del SDK de Compac",
                        status = 400,
                        detail = ex.Message,
                        codigoSdk = ex.Codigo
                    });
                }
            });
    }
}
