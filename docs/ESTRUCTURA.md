# Estructura del proyecto

Solución: `ControlProduccion.slnx` (.NET 10; Api y Compac en x86 — el SDK de Compac es una DLL de 32 bits).

| Proyecto | Responsabilidad |
|---|---|
| `src/ControlProduccion.Dominio` | Entidades y reglas de negocio de producción. Sin dependencias externas. |
| `src/ControlProduccion.Datos` | Base de datos propia (recetas, órdenes, lotes). Referencia a Dominio. |
| `src/ControlProduccion.Compac` | Envoltura del SDK de CONTPAQi Comercial (`Interop/` = DllImport y structs, `Servicios/` = sesión, productos, documentos). |
| `src/ControlProduccion.Api` | Web API ASP.NET Core (HTTPS) que consume el frontend. x86 por el SDK. Referencia a Dominio, Datos y Compac. |
| `tests/ControlProduccion.Pruebas` | Pruebas xUnit del dominio. |

`SDKs_CONTPAQi/` conserva la documentación y los ejemplos originales como referencia (solo lectura).

Flujo de dependencias: Api → (Datos, Compac) → Dominio.

## Notas del SDK dentro de una Web API
- El SDK guarda estado global (sesión, empresa abierta, cursores) y no admite concurrencia: toda llamada pasa por `EjecutorSdk` (un solo hilo STA con cola).
- Ejecutar en la misma máquina que CONTPAQi (usa `C:\Program Files (x86)\Compac\COMERCIAL\`).
- HTTPS local: `dotnet dev-certs https --trust`. Puerto: https://localhost:7080. CORS en `appsettings.json` (`Cors:Origenes`).
