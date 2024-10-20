using CRM.OneMedical.Shared;
using CRM.OneMedical.Shared.Datos;
using CRM.OneMedical.Shared.EntidadesDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRM.OneMedical.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        protected AccesoDatos _db;
        public CitasController(AccesoDatos db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<Respuesta<List<Tbl_Citas>>> Get()
        {
            var respuesta = new Respuesta<List<Tbl_Citas>>();

            respuesta.Datos = await _db.Tbl_Citas.ToListAsync().ConfigureAwait(false);

            return respuesta;

        }

        [HttpGet("{usuarioId}")]
        public async Task<Respuesta<List<Tbl_Citas>>> Get(long usuarioId)
        {
            var respuesta = new Respuesta<List<Tbl_Citas>>() { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                respuesta.Datos = await _db.Tbl_Citas.Where(x => x.PacienteId == usuarioId || x.DoctorId == usuarioId).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                respuesta.Estatus = EstadosDeRespuesta.Error;
            }


            return respuesta;

        }

        // POST api/<CitasController>
        [HttpPost]
        public async Task<Respuesta<string>> Post([FromBody] Tbl_Citas value)
        {
            var respuesta = new Respuesta<string> { Estatus = EstadosDeRespuesta.Correcto };
            try
            {
                value.Fecha = DateTime.Now;
                await _db.Tbl_Citas.AddAsync(value).ConfigureAwait(false);
                //await _db.SaveChangesAsync(true).ConfigureAwait(false);
                //await _db.SaveChangesAsync(false).ConfigureAwait(false);
                _db.SaveChanges();
                respuesta.Mensaje = "Cita guardada correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Datos = ex.ToString();
                respuesta.Mensaje = "Error al guardar su cita";
            }

            return respuesta;
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<Respuesta<Usuario>> GuardarCita(Usuario value)
        {
            var respuesta = new Respuesta<Usuario> { Estatus = EstadosDeRespuesta.Correcto };
            try
            {
                value.Cita.Fecha = DateTime.Now;
                await _db.Tbl_Citas.AddAsync(value.Cita).ConfigureAwait(false);
                //await _db.SaveChangesAsync(true).ConfigureAwait(false);
                //await _db.SaveChangesAsync(false).ConfigureAwait(false);
                _db.SaveChanges();

                value.SintomasDelPaciente.Tbl_CitasId = value.Cita.CitaId;

                await _db.Tbl_SintomasDelPaciente.AddAsync(value.SintomasDelPaciente).ConfigureAwait(false);
                _db.SaveChanges();

                respuesta.Mensaje = "Cita guardada correctamente";
                respuesta.Datos = value;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = "Error al guardar su cita";
            }

            return respuesta;
        }

        // PUT api/<CitasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CitasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
