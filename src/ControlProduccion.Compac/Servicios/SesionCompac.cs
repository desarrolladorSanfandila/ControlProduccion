using System.IO;
using System.Text;
using System.Threading.Tasks;
using ControlProduccion.Compac.Interop;

namespace ControlProduccion.Compac.Servicios
{
    /// <summary>
    /// Sesión única del SDK. El SDK tiene estado global del proceso, así que esta clase debe
    /// registrarse como singleton y todo pasa por <see cref="EjecutorSdk"/>.
    /// </summary>
    public class SesionCompac
    {
        private readonly EjecutorSdk _ejecutor;
        private readonly CompacOptions _opciones;

        // Solo se leen/escriben dentro del hilo del ejecutor.
        private bool _sesionIniciada;
        private string _empresaAbierta;

        public SesionCompac(EjecutorSdk ejecutor, CompacOptions opciones)
        {
            _ejecutor = ejecutor;
            _opciones = opciones;
        }

        public Task<bool> IniciarSesionAsync(string usuario, string contrasenia) =>
            _ejecutor.EjecutarAsync(() =>
            {
                if (_sesionIniciada)
                {
                    // Reiniciar: cierra la empresa y termina la sesión anterior.
                    if (_empresaAbierta != null) MGWServicios.fCierraEmpresa();
                    MGWServicios.fTerminaSDK();
                    _sesionIniciada = false;
                    _empresaAbierta = null;
                }

                if (!Directory.Exists(_opciones.RutaBinarios))
                    throw new CompacException(-1, "No existe la ruta de binarios de Compac: " + _opciones.RutaBinarios);

                MGWServicios.SetDllDirectory(_opciones.RutaBinarios);
                MGWServicios.SetCurrentDirectory(_opciones.RutaBinarios);

                // Orden exigido por el SDK: fInicioSesionSDK, luego fSetNombrePAQ.
                // fInicioSesionSDK no devuelve código; un usuario/contraseña inválido falla en fSetNombrePAQ.
                MGWServicios.fInicioSesionSDK(usuario, contrasenia);
                Verificar(MGWServicios.fSetNombrePAQ(_opciones.NombrePAQ));

                _sesionIniciada = true;
                return true;
            });

        public Task<string> AbrirEmpresaAsync(string directorio) =>
            _ejecutor.EjecutarAsync(() =>
            {
                if (!_sesionIniciada)
                    throw new CompacException(-2, "Primero inicia sesión en el SDK.");

                directorio = string.IsNullOrWhiteSpace(directorio) ? _opciones.DirectorioEmpresa : directorio;
                if (string.IsNullOrWhiteSpace(directorio))
                    throw new CompacException(-3, "Indica el directorio de la empresa.");

                if (_empresaAbierta != null) MGWServicios.fCierraEmpresa();
                _empresaAbierta = null;

                Verificar(MGWServicios.fAbreEmpresa(directorio));
                _empresaAbierta = directorio;
                return directorio;
            });

        private static void Verificar(int codigo)
        {
            if (codigo == 0) return;
            var mensaje = new StringBuilder(512);
            MGWServicios.fError(codigo, mensaje, 512);
            throw new CompacException(codigo, mensaje.ToString());
        }
    }
}
