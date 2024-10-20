using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB
{
    public class Tbl_Citas
    {
        [Key]
        public long CitaId { get; set; }
        public long PacienteId { get; set; }
        public long DoctorId { get; set; }
        public string PacienteNombre { get; set; }
        public string DoctorNombre { get; set; }
        public string NumeroTelefonoPaciente { get; set; }
        public string NumeroTelefonoDoctor { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaCita { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estatus { get; set; }
    }
}
