using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB
{
    public class PropiedadesExtraMedico
    {
        public ConfiguracionConsulta ConfiguracionConsulta { get; set; }
    }

    public class ConfiguracionConsulta
    {
        public long ConfiguracionConsultaId { get; set; }
        public long UsuarioId { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan DuracionConsultas { get; set; }
        public List<ConsultasPorMedico> HorariosDeConsulta { get; set; }
    }

    public class ConsultasPorMedico
    {
        public long ConsultasPorMedicoId { get; set; }
        public long ConfiguracionConsultaId { get; set; }
        public TimeSpan HoraInicioCita { get; set; }
        public TimeSpan HoraFinCita { get; set; }
        public bool EstaOcupado { get; set; }
    }
}
