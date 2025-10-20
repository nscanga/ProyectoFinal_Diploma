using System;

namespace BLL.Commands
{
    /// <summary>
    /// Interfaz base para el patr�n Command
    /// </summary>
    public interface ICommand
    {
        void Execute();
        void Undo();
        bool CanExecute();
    }

    /// <summary>
    /// Interfaz para comandos que devuelven un resultado
    /// </summary>
    /// <typeparam name="TResult">Tipo del resultado</typeparam>
    public interface ICommand<out TResult>
    {
        TResult Execute();
        bool CanExecute();
    }

    /// <summary>
    /// Comando base abstracto
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        protected Exception _lastException;

        public abstract void Execute();
        public abstract void Undo();
        public virtual bool CanExecute() => true;

        protected virtual void OnCommandExecuted()
        {
            // Hook para l�gica post-ejecuci�n
        }

        protected virtual void OnCommandFailed(Exception ex)
        {
            _lastException = ex;
            // Hook para manejo de errores
        }
    }
}