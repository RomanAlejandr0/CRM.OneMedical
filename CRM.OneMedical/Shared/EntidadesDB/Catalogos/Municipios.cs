using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB.Catalogos
{
    public class Municipios
    {
        [Key]
        public string c_Municipio { get; set; }

        public string c_Estado { get; set; }

        public string Descripcion { get; set; }
    }
}
