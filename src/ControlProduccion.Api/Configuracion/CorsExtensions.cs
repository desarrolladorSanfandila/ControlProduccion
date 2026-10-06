using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControlProduccion.Api.Configuracion
{
    public static class CorsExtensions
    {
        public const string PoliticaFrontend = "Frontend";

        public static IServiceCollection AddCorsFrontend(this IServiceCollection services, IConfiguration config)
        {
            var origenes = config.GetSection("Cors:Origenes").Get<string[]>() ?? Array.Empty<string>();
            return services.AddCors(o => o.AddPolicy(PoliticaFrontend, p =>
                p.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod()));
        }
    }
}
