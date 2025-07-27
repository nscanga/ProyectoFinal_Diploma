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
    public static class IdiomaService
    {
        private static List<IIdiomaObserver> observers = new List<IIdiomaObserver>();

        // Método para agregar observadores
        public static void Subscribe(IIdiomaObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        // Método para remover observadores
        public static void Unsubscribe(IIdiomaObserver observer)
        {
            observers.Remove(observer);
        }

        // Método para notificar a todos los observadores cuando cambia el idioma
        private static void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.UpdateIdioma();  // Notificar al observador de la UI
            }
        }

        // Método para traducir una clave específica
        public static string Translate(string key)
        {
            return IdiomaLogic.Translate(key);
        }

        // Guardar el idioma del usuario y notificar a los observadores
        public static void SaveUserLanguage(string languageCode)
        {
            IdiomaLogic.SaveUserLanguage(languageCode);
            NotifyObservers();  // Notificar cuando cambia el idioma
        }

        // Cargar el idioma guardado
        public static string LoadUserLanguage()
        {
            return IdiomaLogic.LoadUserLanguage();
        }

        // Traducir un formulario completo
        public static void TranslateForm(Control control)
        {
            IdiomaLogic.TranslateForm(control);
        }
    }
}
