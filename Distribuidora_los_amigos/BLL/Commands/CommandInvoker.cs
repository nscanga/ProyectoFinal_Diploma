using BLL.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Commands
{
    /// <summary>
    /// Invoker para gestionar la ejecución de comandos
    /// </summary>
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();
        private readonly int _maxHistorySize;

        /// <summary>
        /// Crea un invocador indicando la cantidad máxima de comandos a conservar en el historial.
        /// </summary>
        /// <param name="maxHistorySize">Límite de comandos almacenados para deshacer.</param>
        public CommandInvoker(int maxHistorySize = 50)
        {
            _maxHistorySize = maxHistorySize;
        }

        /// <summary>
        /// Ejecuta un comando y lo guarda en el historial
        /// </summary>
        public void ExecuteCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!command.CanExecute())
                throw new InvalidOperationException("El comando no puede ejecutarse en el estado actual.");

            try
            {
                command.Execute();
                
                // Agregar al historial, limitando el tamaño
                _commandHistory.Push(command);
                if (_commandHistory.Count > _maxHistorySize)
                {
                    var excess = _commandHistory.Count - _maxHistorySize;
                    var tempStack = new Stack<ICommand>();
                    
                    // Mantener solo los comandos más recientes
                    for (int i = 0; i < _maxHistorySize; i++)
                    {
                        tempStack.Push(_commandHistory.Pop());
                    }
                    
                    _commandHistory.Clear();
                    while (tempStack.Count > 0)
                    {
                        _commandHistory.Push(tempStack.Pop());
                    }
                }
            }
            catch (Exception ex)
            {
                // Log del error - Comentado porque BLL no debe referenciar Service
                // Service.Facade.LoggerService.WriteException(ex);
                throw;
            }
        }

        /// <summary>
        /// Ejecuta comando con resultado
        /// </summary>
        public TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!command.CanExecute())
                throw new InvalidOperationException("El comando no puede ejecutarse en el estado actual.");

            try
            {
                return command.Execute();
            }
            catch (Exception ex)
            {
                // Log del error - Comentado porque BLL no debe referenciar Service
                // Service.Facade.LoggerService.WriteException(ex);
                throw;
            }
        }

        /// <summary>
        /// Deshace el último comando ejecutado
        /// </summary>
        public void UndoLastCommand()
        {
            if (_commandHistory.Count == 0)
                throw new InvalidOperationException("No hay comandos para deshacer.");

            var lastCommand = _commandHistory.Pop();
            try
            {
                lastCommand.Undo();
            }
            catch (Exception ex)
            {
                // Volver a agregar el comando al historial si falla el undo
                _commandHistory.Push(lastCommand);
                // Log del error - Comentado porque BLL no debe referenciar Service
                // Service.Facade.LoggerService.WriteException(ex);
                throw;
            }
        }

        /// <summary>
        /// Indica si hay comandos que se pueden deshacer
        /// </summary>
        public bool CanUndo => _commandHistory.Count > 0;

        /// <summary>
        /// Limpia el historial de comandos
        /// </summary>
        public void ClearHistory()
        {
            _commandHistory.Clear();
        }

        /// <summary>
        /// Obtiene el número de comandos en el historial
        /// </summary>
        public int HistoryCount => _commandHistory.Count;
    }
}
