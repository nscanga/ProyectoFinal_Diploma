using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    /// <summary>
    /// Representa un usuario del sistema con sus credenciales y accesos asignados.
    /// </summary>
    public class Usuario
    {
        public Guid IdUsuario { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email{ get; set; }

        public string RecoveryToken { get; set; } // El token de recuperación
        public DateTime TokenExpiration { get; set; } // El momento de expiración del token

        public string Lenguaje { get; set; } = "en";

        public string Estado { get; set; } // Deshabilitado  habilitado

        public List<Acceso> Accesos = new List<Acceso>();


        /// <summary>
        /// Crea un usuario generando automáticamente un nuevo identificador.
        /// </summary>
        public Usuario()
        {
            IdUsuario = Guid.NewGuid();  // Esto genera un nuevo Id cada vez
        }

        /// <summary>
        /// Crea un usuario reutilizando el identificador proporcionado.
        /// </summary>
        /// <param name="idUsuario">Identificador existente.</param>
        public Usuario(Guid idUsuario)
        {
            this.IdUsuario = idUsuario;
        }
        /// <summary>
        /// Obtiene todas las patentes asociadas al usuario, recorriendo familias anidadas.
        /// </summary>
        public List<Patente> GetPatentes()
        {
            List<Patente> patentes = new List<Patente>();

            GetAllPatentes(Accesos, patentes);

            return patentes;
        }

        /// <summary>
        /// Recorre recursivamente la jerarquía de accesos para recopilar patentes únicas.
        /// </summary>
        private void GetAllPatentes(List<Acceso> accesos, List<Patente> patentesReturn)
        {
            foreach (var acceso in accesos)
            {
                //Cuál sería mi condición de corte?
                //Significa que estoy ante un elemento de tipo Leaf, Hoja, Primitivo
                if (acceso.GetCount() == 0)
                {
                    //Podría pasar que la patente ya esté agregada (Similar a un distinct)
                    if (!patentesReturn.Any(o => o.Id == acceso.Id))
                        patentesReturn.Add(acceso as Patente);
                }
                else
                {
                    //Tengo que tratar a mi "acceso" como si fuese una familia
                    GetAllPatentes((acceso as Familia).Accesos, patentesReturn);
                }
            }
        }

        /// <summary>
        /// Obtiene todas las familias asociadas al usuario.
        /// </summary>
        public List<Familia> GetFamilias()
        {

            List<Familia> familias = new List<Familia>();

            GetAllFamilias(Accesos, familias);

            return familias;

        }

        /// <summary>
        /// Recorre la jerarquía de accesos agregando familias sin duplicados.
        /// </summary>
        private void GetAllFamilias(List<Acceso> accesos, List<Familia> famililaReturn)
        {
            foreach (var acceso in accesos)
            {
                //Cuál sería mi condición de corte?
                //Significa que estoy ante un elemento de tipo Composite
                if (acceso.GetCount() > 0)
                {
                    if (!famililaReturn.Any(o => o.Id == acceso.Id))
                        famililaReturn.Add(acceso as Familia);

                    GetAllFamilias((acceso as Familia).Accesos, famililaReturn);
                }
            }
        }
    }
}
