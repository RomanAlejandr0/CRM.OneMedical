using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB
{
    public class Domicilio
    {
        [Key]
        public long DomicilioId { get; set; }
        public long UbicacionId { get; set; }
        public long EmisorId { get; set; }
        public long ReceptorId { get; set; }
        public long FiguraId { get; set; }
        public string Calle { get; set; }

        public string NumeroExterior { get; set; }
        public string Colonia { get; set; }
        public string Localidad { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        //[Required(ErrorMessage = "El codigo postal es requerido")]
        public string CodigoPostal { get; set; }

        public DateTime Fecha { get; set; }
    }
}
