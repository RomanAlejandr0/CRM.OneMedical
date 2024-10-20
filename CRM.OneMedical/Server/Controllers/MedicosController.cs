using CRM.OneMedical.Shared;
using CRM.OneMedical.Shared.Datos;
using CRM.OneMedical.Shared.EntidadesDB;
using CRM.OneMedical.Shared.Peticiones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Numerics;

namespace CRM.OneMedical.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly AccesoDatos db;

        public MedicosController(AccesoDatos db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<Respuesta<List<Usuario>>> CargarDoctores([FromBody] string value)
        {
            var respuesta = new Respuesta<List<Usuario>> { Estatus = EstadosDeRespuesta.Correcto };
            var doctore = new List<Usuario>();
            ConfiguracionConsulta? configuracionConsulta = new ConfiguracionConsulta();
            List<ConsultasPorMedico>? horariosDeConsulta = new List<ConsultasPorMedico>();

            try
            {
                var doctores = await db.Tbl_Usuarios.Where(x => x.Perfil == (short)PerfilesUsuarios.Medico).ToListAsync();

                foreach (var doctor in doctores)
                {

                    doctor.ConfiguracionConsulta = await db.Tbl_ConfiguracionConsulta.FirstOrDefaultAsync(x => x.UsuarioId == doctor.UsuarioId);


                    if (doctor.ConfiguracionConsulta != null)
                    {
                      
                        doctor.HorariosDeConsulta = await ObtenerCitasDisponibles(new ConsultaCitas { UsuarioId = doctor.UsuarioId, Fecha = DateTime.Now}, doctor.ConfiguracionConsulta.ConfiguracionConsultaId);
                    }
                    else
                    {
                        doctor.HorariosDeConsulta = new List<ConsultasPorMedico>();
                    }
                }

                respuesta.Datos = doctores;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al consultar los doctores";
            }


            return respuesta;
        }


        [HttpPost("ObtenerCitasDisponibles")]
        private async Task<List<ConsultasPorMedico>> ObtenerCitasDisponibles([FromBody]ConsultaCitas value, long ConfiguracionConsultaId)
        {
            var citasDisponibles = new List<ConsultasPorMedico>();

            citasDisponibles = await db.Tbl_ConsultasPorMedico.
                           Where(x => x.ConfiguracionConsultaId == ConfiguracionConsultaId && x.EstaOcupado == false).ToListAsync();


            var citas = await db.Tbl_Citas.Where(x => x.DoctorId == value.UsuarioId && x.FechaCita == value.Fecha.Date).ToListAsync();

            if (citas.Count != null)
            {
                foreach (var cita in citas)
                {
                    citasDisponibles.RemoveAll(x => x.HoraInicioCita == cita.HoraInicio);
                }
            }

            return citasDisponibles;
        }

        [HttpGet("CargarConfiguracionConsulta/{usuarioId}")]
        public async Task<Respuesta<ConfiguracionConsulta>> Get(long usuarioId)
        {
            var respuesta = new Respuesta<ConfiguracionConsulta> { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                var configuracionConsulta = new ConfiguracionConsulta();

                configuracionConsulta = await db.Tbl_ConfiguracionConsulta.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

                configuracionConsulta.HorariosDeConsulta = await db.Tbl_ConsultasPorMedico.Where(x => x.ConfiguracionConsultaId == configuracionConsulta.ConfiguracionConsultaId).ToListAsync();

                respuesta.Datos = configuracionConsulta;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al consultar el usuario";
            }

            return respuesta;
        }


        //[HttpGet("CargarConfiguracionConsulta/{usuarioId}")]
        //public async Task<Respuesta<ConfiguracionConsulta>> EvaluarCitas(DateTime date)
        //{
        //    bool haycitas = await db.Tbl_Citas.FindAsync(x => x.FechaCita.Date == date.Date);
        //}

        [HttpPost]
        [Route("{action}")]
        public async Task<Respuesta<long>> GuardarJornadaLaboral([FromBody] ConfiguracionConsulta configuracionConsulta)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto, Mensaje = "Guardado Correctamente" };

            if (configuracionConsulta.ConfiguracionConsultaId == 0)
            {
                respuesta = await GuardarUsuario(configuracionConsulta);

            }
            else
            {
                respuesta = await EditarUsuario(configuracionConsulta);
            }

            return respuesta;
        }

        public async Task<Respuesta<long>> GuardarUsuario(ConfiguracionConsulta configuracionConsulta)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto, Mensaje = "Guardado Correctamente" };

            try
            {
                await db.Tbl_ConfiguracionConsulta.AddAsync(configuracionConsulta);
                await db.SaveChangesAsync(true);

                configuracionConsulta.HorariosDeConsulta.
                    ForEach(x => x.ConfiguracionConsultaId = configuracionConsulta.ConfiguracionConsultaId);

                foreach (var horarioConsulta in configuracionConsulta.HorariosDeConsulta)
                {
                    await db.Tbl_ConsultasPorMedico.AddAsync(horarioConsulta);

                    await db.SaveChangesAsync(true);
                }

                respuesta.Datos = configuracionConsulta.ConfiguracionConsultaId;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Ocurrio un error al guardar la empresa: {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
            }

            return respuesta;
        }

        public async Task<Respuesta<long>> EditarUsuario(ConfiguracionConsulta configuracionConsulta)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                db.Attach(configuracionConsulta).State = EntityState.Modified;

                await db.SaveChangesAsync();

                foreach (var horarioConsulta in configuracionConsulta.HorariosDeConsulta)
                {
                    db.Attach(horarioConsulta).State = EntityState.Modified;

                    await db.SaveChangesAsync(true);
                }


                respuesta.Datos = configuracionConsulta.ConfiguracionConsultaId;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al guardar al actualizar el usuario ID: {configuracionConsulta.UsuarioId}";
            }

            return respuesta;
        }

    }
}
