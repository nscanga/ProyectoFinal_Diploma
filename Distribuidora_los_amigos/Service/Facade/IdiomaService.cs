using Service.DAL.Contracts;
using Service.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.Facade
{
    /// <summary>
    /// Gestiona las operaciones relacionadas con la traducción y los observadores de idioma.
    /// </summary>
    public static class IdiomaService
    {
        private static List<IIdiomaObserver> observers = new List<IIdiomaObserver>();

        // Método para agregar observadores
        /// <summary>
        /// Registra un nuevo observador de cambios de idioma.
        /// </summary>
        public static void Subscribe(IIdiomaObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        // Método para remover observadores
        /// <summary>
        /// Elimina un observador previamente suscrito.
        /// </summary>
        public static void Unsubscribe(IIdiomaObserver observer)
        {
            observers.Remove(observer);
        }

        // Método para notificar a todos los observadores cuando cambia el idioma
        /// <summary>
        /// Notifica a todos los observadores registrados que el idioma ha cambiado.
        /// </summary>
        private static void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.UpdateIdioma();  // Notificar al observador de la UI
            }
        }

        // Método para traducir una clave específica
        /// <summary>
        /// Traduce una clave utilizando la lógica de idiomas.
        /// </summary>
        public static string Translate(string key)
        {
            return IdiomaLogic.Translate(key);
        }

        // Guardar el idioma del usuario y notificar a los observadores
        /// <summary>
        /// Guarda el idioma seleccionado por el usuario y notifica a los observadores.
        /// </summary>
        public static void SaveUserLanguage(string languageCode)
        {
            IdiomaLogic.SaveUserLanguage(languageCode);
            NotifyObservers();  // Notificar cuando cambia el idioma
        }

        // Cargar el idioma guardado
        /// <summary>
        /// Recupera el idioma almacenado para el usuario.
        /// </summary>
        public static string LoadUserLanguage()
        {
            return IdiomaLogic.LoadUserLanguage();
        }

        // Traducir un formulario completo
        /// <summary>
        /// Traduce todos los controles de un formulario utilizando sus etiquetas.
        /// </summary>
        public static void TranslateForm(Control control)
        {
            IdiomaLogic.TranslateForm(control);
        }
    }
}
