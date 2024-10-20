using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRM.OneMedical.Shared.EntidadesDB.AdministracionPerfilesTareas
{
public class Tbl_Adm_Perfil_Tarea

    {  
        [Key]
       public int PerfilId { get; set; }
       public int TareaId { get; set; }

    }
}
