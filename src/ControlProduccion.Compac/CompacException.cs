using System;

namespace ControlProduccion.Compac
{
    /// <summary>Error devuelto por el SDK (código distinto de 0) o estado inválido de la sesión.</summary>
    public class CompacException : Exception
    {
        public int Codigo { get; }

        public CompacException(int codigo, string mensaje) : base(mensaje)
        {
            Codigo = codigo;
        }
    }
}
