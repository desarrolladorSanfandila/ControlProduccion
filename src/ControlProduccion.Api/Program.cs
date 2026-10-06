using ControlProduccion.Api.Configuracion;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCorsFrontend(builder.Configuration);
builder.Services.AddCompac(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseErroresCompac();
app.UseHttpsRedirection();
app.UseCors(CorsExtensions.PoliticaFrontend);
app.UseAuthorization();
app.MapControllers();

app.Run();
