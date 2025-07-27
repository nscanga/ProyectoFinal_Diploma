using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DOMAIN
{
    public class Bitacora
    {
        public Guid Id_Log { get; set; } // Identificador único
        public DateTime Fecha { get; set; } // Fecha y hora de la entrada
        public string TraceLevel { get; set; } // Nivel de severidad (Info, Warning, Error, etc.)
        public string Mensaje { get; set; } // Mensaje del log
        public string StackTrace { get; set; } // Información de rastreo (opcional)
    }
}
