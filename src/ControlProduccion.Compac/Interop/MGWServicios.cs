using System.Runtime.InteropServices;
using System.Text;

namespace ControlProduccion.Compac.Interop
{
    /// <summary>
    /// Declaraciones de MGWServicios.DLL (SDK CONTPAQi Comercial). Se agregan aquí solo las funciones
    /// que la API usa; el catálogo completo está en SDKs_CONTPAQi/.../SDK_C#/MGWServicios.cs.
    /// </summary>
    internal static class MGWServicios
    {
        [DllImport("KERNEL32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int SetCurrentDirectory(string ruta);

        [DllImport("KERNEL32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool SetDllDirectory(string ruta);

        // Debe llamarse ANTES de fSetNombrePAQ (ver "Uso de fInicioSesion.pdf").
        [DllImport("MGWServicios.DLL", CharSet = CharSet.Ansi)]
        public static extern void fInicioSesionSDK(string usuario, string contrasenia);

        [DllImport("MGWServicios.DLL", CharSet = CharSet.Ansi)]
        public static extern int fSetNombrePAQ(string nombrePAQ);

        [DllImport("MGWServicios.DLL")]
        public static extern void fTerminaSDK();

        [DllImport("MGWServicios.DLL", CharSet = CharSet.Ansi)]
        public static extern void fError(int numeroError, StringBuilder mensaje, int longitud);

        [DllImport("MGWServicios.DLL", CharSet = CharSet.Ansi)]
        public static extern int fAbreEmpresa(string directorio);

        [DllImport("MGWServicios.DLL")]
        public static extern void fCierraEmpresa();
    }
}
