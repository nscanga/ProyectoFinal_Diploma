using Service.DAL.Implementations.SqlServer.Helpers;
using Service.DOMAIN;
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
    /// Fachada que coordina la gestión de familias y sus patentes con validaciones adicionales.
    /// </summary>
    public static class FamiliaService
    {
        private static readonly FamiliaLogic _familiaLogic = new FamiliaLogic();
        private static readonly FamiliaRepository _familiaDAL = new FamiliaRepository();


        /// <summary>
        /// Crea una familia validando su nombre y que tenga patentes asociadas.
        /// </summary>
        public static void CrearFamiliaConPatentes(string nombreFamilia, List<Patente> patentes)
        {
            if (string.IsNullOrWhiteSpace(nombreFamilia))
            {
                string messageKey = "El nombre de la familia no puede estar vacío.:";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new ArgumentException(translatedMessage);
            }
            if (patentes == null || !patentes.Any())
            {
                string messageKey = "Debe haber al menos una patente asociada a la familia.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new ArgumentException(translatedMessage);
                
            }
            var familia = new Familia { Nombre = nombreFamilia };
            foreach (var patente in patentes)
            {
                familia.Add(patente);
            }
            if (_familiaDAL.ExisteFamilia(familia.Nombre))
            {
                string messageKey = "Ya existe una familia con el nombre";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new ArgumentException(translatedMessage +  familia.Nombre );
               
            }

            _familiaLogic.CrearFamiliaConPatentes(familia);
        }



        /// <summary>
        /// Asocia una familia a un usuario siempre que no tenga otra previamente asignada.
        /// </summary>
        public static void AsignarFamiliaAUsuario(Guid usuarioId, Familia familia)
        {
            if(_familiaDAL.ExisteFamiliaParaUsuario(usuarioId))
            {
                string messageKey = "El usuario ya tiene una familia asignada.";
                string translatedMessage = TranslateMessageKey(messageKey);
                throw new Exception(translatedMessage);
            }
            _familiaLogic.AsignarFamiliaAUsuario(usuarioId, familia);


        }

        /// <summary>
        /// Actualiza los datos y patentes de una familia.
        /// </summary>
        public static void ActualizarFamilia(Familia familia)
        {
            _familiaLogic.ActualizarFamilia(familia);
        }

        /// <summary>
        /// Sustituye las familias asociadas a un usuario.
        /// </summary>
        public static void ActualizarFamiliasDeUsuario(Guid usuarioId, List<Familia> familias)
        {
            _familiaLogic.ActualizarFamiliasDeUsuario(usuarioId, familias);
        }
        /// <summary>
        /// Obtiene todas las familias disponibles.
        /// </summary>
        public static List<Familia> GetAllFamilias()
        {
            return _familiaLogic.GetAllFamilias();
        }
        /// <summary>
        /// Lista todas las patentes registradas.
        /// </summary>
        public static List<Patente> GetAllPatentes()
        {
            return _familiaLogic.GetAllPatentes();
        }
        /// <summary>
        /// Traduce una clave de mensaje usando el servicio de idioma.
        /// </summary>
        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
    }
}
