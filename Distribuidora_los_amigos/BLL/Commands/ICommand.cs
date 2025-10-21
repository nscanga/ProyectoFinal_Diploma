using System;

namespace BLL.Commands
{
    /// <summary>
    /// Interfaz base para el patrón Command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Ejecuta la acción encapsulada por el comando.
        /// </summary>
        void Execute();

        /// <summary>
        /// Revierte los efectos producidos por la ejecución del comando.
        /// </summary>
        void Undo();

        /// <summary>
        /// Indica si el comando puede ejecutarse en el estado actual.
        /// </summary>
        bool CanExecute();
    }

    /// <summary>
    /// Interfaz para comandos que devuelven un resultado.
    /// </summary>
    /// <typeparam name="TResult">Tipo del resultado retornado por el comando.</typeparam>
    public interface ICommand<out TResult>
    {
        /// <summary>
        /// Ejecuta el comando y devuelve su resultado.
        /// </summary>
        /// <returns>Valor producido por la ejecución.</returns>
        TResult Execute();

        /// <summary>
        /// Indica si el comando está en condiciones de ejecutarse.
        /// </summary>
        /// <returns>True si puede ejecutarse; de lo contrario false.</returns>
        bool CanExecute();
    }

    /// <summary>
    /// Comando base abstracto que provee hooks para reaccionar a la ejecución.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        protected Exception _lastException;

        /// <inheritdoc />
        public abstract void Execute();

        /// <inheritdoc />
        public abstract void Undo();

        /// <summary>
        /// Verifica si el comando puede ejecutarse; por defecto siempre es posible.
        /// </summary>
        /// <returns>True cuando se permite la ejecución.</returns>
        public virtual bool CanExecute() => true;

        /// <summary>
        /// Hook invocado luego de ejecutar el comando exitosamente.
        /// </summary>
        protected virtual void OnCommandExecuted()
        {
            // Hook para lógica post-ejecución
        }

        /// <summary>
        /// Hook invocado cuando la ejecución del comando produce un error.
        /// </summary>
        /// <param name="ex">Excepción capturada durante la ejecución.</param>
        protected virtual void OnCommandFailed(Exception ex)
        {
            _lastException = ex;
            // Hook para manejo de errores
        }
    }
}
