using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB
{
    public class SintomasDelPaciente
    {
        public long SintomasDelPacienteId { get; set; }
        public long UsuarioId { get; set; }
        public long Tbl_CitasId { get; set; }
        public string InicioDeLosSintomas { get; set; }
        public string SintomasPrincipales { get; set; }
        public string ComoEmpezo { get; set; }
        public string ComoEvoluciono { get; set; }

    }
}
