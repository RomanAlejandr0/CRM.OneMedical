using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared
{
    public class Respuesta<T>
    {
        public EstadosDeRespuesta Estatus { get; set; }

        public string Mensaje { get; set; }

        public T Datos { get; set; }
    }
}
