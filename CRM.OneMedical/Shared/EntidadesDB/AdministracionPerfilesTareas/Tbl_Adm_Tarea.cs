using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.OneMedical.Shared.EntidadesDB.AdministracionPerfilesTareas
{
  public class Tbl_Adm_Tarea
    {
        [Key]
        public int TareaId { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public byte Es_Activo { get; set; }
    }
}
