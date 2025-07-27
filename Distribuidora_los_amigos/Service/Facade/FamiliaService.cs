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
    public static class FamiliaService
    {
        private static readonly FamiliaLogic _familiaLogic = new FamiliaLogic();
        private static readonly FamiliaRepository _familiaDAL = new FamiliaRepository();
    

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

        public static void ActualizarFamilia(Familia familia)
        {
            _familiaLogic.ActualizarFamilia(familia);
        }

        public static void ActualizarFamiliasDeUsuario(Guid usuarioId, List<Familia> familias)
        {
            _familiaLogic.ActualizarFamiliasDeUsuario(usuarioId, familias);
        }
        public static List<Familia> GetAllFamilias()
        {
            return _familiaLogic.GetAllFamilias();
        }
        public static List<Patente> GetAllPatentes()
        {
            return _familiaLogic.GetAllPatentes();
        }
        private static string TranslateMessageKey(string messageKey)
        {
            return IdiomaService.Translate(messageKey);
        }
    }
}
