using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.OneMedical.Shared.EntidadesDB.AdministracionPerfilesTareas
{
    public class Tbl_Adm_Perfil
    {   [Key]
    [Required (ErrorMessage ="el perfil es requerido")]
        public int PerfilId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public byte Es_Activo { get; set; }
    }
}
