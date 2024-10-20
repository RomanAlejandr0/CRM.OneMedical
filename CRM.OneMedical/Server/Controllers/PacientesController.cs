using CRM.OneMedical.Shared;
using CRM.OneMedical.Shared.Datos;
using CRM.OneMedical.Shared.EntidadesDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.OneMedical.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PacientesController: ControllerBase
    {
        private readonly AccesoDatos db;

        public PacientesController(AccesoDatos db)
        {
            this.db = db;
        }

        [HttpGet("CargarInformacionAdicionalPaciente/{usuarioId}")]
        public async Task<Respuesta<InformacionAdicionalDelPaciente>> Get(long usuarioId)
        {
            var respuesta = new Respuesta<InformacionAdicionalDelPaciente> { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                var informacionAdicionalPaciente = new InformacionAdicionalDelPaciente();

                informacionAdicionalPaciente = await db.Tbl_InformacionAdicionalDelPaciente.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

                respuesta.Datos = informacionAdicionalPaciente;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al consultar el usuario";
            }

            return respuesta;
        }


    }
}
