using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.OneMedical.Shared.EntidadesDB
{
    public class Usuario
    {
        [Key]
        public long UsuarioId { get; set; }
        public short Perfil { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El Apellido es obligatorio")]
        public string Apellidos { get; set; } //TODO: Apellidos juntos o separados por Apellido Paterno y Apelllido Materno
        
        public Domicilio? Domicilio { get; set; } //TODO: Diferencia entre Direccion y Lugar de Residencia || ES LA DIRECCION
        
        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string Telefono { get; set; } //TODO: Solo el numero de Telefono Personal o algun numero de telefono extra
        
        [Required(ErrorMessage = "El Email es obligatorio")]
        public string EMail { get; set; }
        
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        public DateTime Fecha { get; set; }
        public Tbl_Citas? Cita { get; set; }
        public List<Tbl_Citas>? Citas { get; set; }

        //PACIENTES
        public InformacionAdicionalDelPaciente? InformacionAdicionalDelPaciente { get; set; }
        public SintomasDelPaciente? SintomasDelPaciente { get; set; }

        //MEDICOS
        public ConfiguracionConsulta? ConfiguracionConsulta { get; set; }
        public List<ConsultasPorMedico>? HorariosDeConsulta { get; set; }

    }

    public class UserInfo
    {
        [Required(ErrorMessage = "El Correo es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es requerida")]
        public string Password { get; set; }
    }

    public class UserToken
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
