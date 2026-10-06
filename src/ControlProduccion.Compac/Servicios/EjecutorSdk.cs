using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ControlProduccion.Compac.Servicios
{
    /// <summary>
    /// El SDK de Compac mantiene estado global (sesión, empresa abierta, cursores) y no admite
    /// llamadas concurrentes. Toda llamada al SDK se encola aquí y corre en un único hilo.
    /// </summary>
    public sealed class EjecutorSdk : IDisposable
    {
        private readonly BlockingCollection<Action> _cola = new BlockingCollection<Action>();
        private readonly Thread _hilo;

        public EjecutorSdk()
        {
            _hilo = new Thread(() =>
            {
                foreach (var accion in _cola.GetConsumingEnumerable()) accion();
            })
            { IsBackground = true, Name = "SDK-Compac" };
            _hilo.SetApartmentState(ApartmentState.STA);
            _hilo.Start();
        }

        public Task<T> EjecutarAsync<T>(Func<T> funcion)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            _cola.Add(() =>
            {
                try { tcs.SetResult(funcion()); }
                catch (Exception ex) { tcs.SetException(ex); }
            });
            return tcs.Task;
        }

        public void Dispose() => _cola.CompleteAdding();
    }
}
