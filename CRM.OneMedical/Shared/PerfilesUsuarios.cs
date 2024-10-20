using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared
{
    public enum PerfilesUsuarios: int
    {
        Administrador = 1,
        Recepcionista = 2,
        Medico = 3,
        Enfermera = 4,
        Paciente = 5
    }
}
