using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CRM.OneMedical.Shared.EntidadesDB;
using CRM.OneMedical.Shared.EntidadesDB.AdministracionPerfilesTareas;
using CRM.OneMedical.Shared.EntidadesDB.Catalogos;
using Microsoft.EntityFrameworkCore;


namespace CRM.OneMedical.Shared.Datos
{
    public class AccesoDatos : DbContext
    {
        public DbSet<Usuario> Tbl_Usuarios { get; set; }
        public DbSet<Tbl_Citas> Tbl_Citas { get; set; }

        
        public DbSet<InformacionAdicionalDelPaciente> Tbl_InformacionAdicionalDelPaciente { get; set; }
        public DbSet<SintomasDelPaciente> Tbl_SintomasDelPaciente { get; set; }

        public DbSet<ConfiguracionConsulta> Tbl_ConfiguracionConsulta { get; set; }
        public DbSet<ConsultasPorMedico> Tbl_ConsultasPorMedico { get; set; }



        #region Catalogos
        public DbSet<Estados> Estados { get; set; }
        public DbSet<Localidades> Localidades { get; set; }
        public DbSet<Municipios> Municipios { get; set; }
        public DbSet<Colonias> Colonias { get; set; }
        public DbSet<CodigosPostales> CodigosPostales { get; set; }
        #endregion


        #region Administracion Perfil y Tareas
        public DbSet<Tbl_Adm_Perfil> Tbl_Adm_Perfil { get; set; }
        public DbSet<Tbl_Adm_Tarea> Tbl_Adm_Tarea { get; set; }
        public DbSet<Tbl_Adm_Perfil_Tarea> Tbl_Adm_Perfil_Tarea { get; set; }
        #endregion

        public AccesoDatos(DbContextOptions<AccesoDatos> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().Ignore(x => x.Domicilio);
            modelBuilder.Entity<Usuario>().Ignore(x => x.Cita);
            modelBuilder.Entity<Usuario>().Ignore(x => x.Citas);
            modelBuilder.Entity<Usuario>().Ignore(x => x.InformacionAdicionalDelPaciente);
            modelBuilder.Entity<Usuario>().Ignore(x => x.SintomasDelPaciente);
            modelBuilder.Entity<Usuario>().Ignore(x => x.ConfiguracionConsulta);
            modelBuilder.Entity<Usuario>().Ignore(x => x.HorariosDeConsulta);

            modelBuilder.Entity<ConfiguracionConsulta>().Ignore(x => x.HorariosDeConsulta);

            modelBuilder.Entity<CodigosPostales>().Ignore(u => u.Estado);
            modelBuilder.Entity<CodigosPostales>().Ignore(u => u.Localidad);
            modelBuilder.Entity<CodigosPostales>().Ignore(u => u.Municipio);
            modelBuilder.Entity<CodigosPostales>().Ignore(u => u.Colonias);
        }

    }
}
