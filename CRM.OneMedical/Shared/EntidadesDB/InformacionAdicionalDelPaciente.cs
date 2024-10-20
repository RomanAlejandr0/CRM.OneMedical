using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB
{
    public class InformacionAdicionalDelPaciente
    {
        [Key]
        public long InformacionAdicionalDelPacienteId { get; set; }
        public long UsuarioId { get; set; }
       
        [Required(ErrorMessage = "El campo Nacionalidad es obligatorio")]
        public string Nacionalidad { get; set; }
       
        [Required(ErrorMessage = "El Campo Edad es obligatorio")]
        public short Edad { get; set; }
       
        [Required(ErrorMessage = "El Campo Genero es obligatorio")]
        public string Genero { get; set; }
        
        [Required(ErrorMessage = "El Campo Estado Civil es obligatorio")]
        public string EstadoCivil { get; set; }
        
        [Required(ErrorMessage = "El Campo Ocupacion es obligatorio")]
        public string Ocupacion { get; set; }
        
        [Required(ErrorMessage = "El campo Habitos de Vida es obligatorio")]
        public string HabitosDeVida { get; set; } //TODO: Algun ejemplo de esto
        
        [Required(ErrorMessage = "El Campo Numero de Hijos es obligatorio")]
        public short NumeroDeHijos { get; set; }
        
        [Required(ErrorMessage = "El Campo Motivo Consulta es obligatorio")]
        public string MotivoDeConsulta { get; set; }
    }
}
