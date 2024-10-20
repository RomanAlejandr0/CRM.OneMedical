using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB.Catalogos
{
    public class Colonias
    {
        [Key]
        public string c_Colonia { get; set; }

        public string c_CodigoPostal { get; set; }

        public string NombreAsentamiento { get; set; }
    }
}
