using CRM.OneMedical.Shared;
using CRM.OneMedical.Shared.Datos;
using CRM.OneMedical.Shared.EntidadesDB;
using CRM.OneMedical.Shared.EntidadesDB.Catalogos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRM.Financiera.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private AccesoDatos datos;
        public CatalogosController(AccesoDatos db)
        {
            this.datos = db;
        }

        [HttpPost("ValidarCodigoPostal")]
        public async Task<Respuesta<List<CodigosPostales>>> ValidarCodigoPostal([FromBody] string value)
        {
            var respuesta = new Respuesta<List<CodigosPostales>> { Estatus = EstadosDeRespuesta.Correcto, Mensaje = "Facturando" };
            try
            {
                respuesta = await ValidarCodigoPostalMetodo(value).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al buscar el codigo postal {ex.Message}";
            }
            return respuesta;
        }


        public async Task<Respuesta<List<CodigosPostales>>> ValidarCodigoPostalMetodo(string value)
        {
            var respuesta = new Respuesta<List<CodigosPostales>>() { Estatus = EstadosDeRespuesta.Correcto };
            var codigoPostalDB = datos.Colonias.Any(cp => cp.c_CodigoPostal == value);
            if (!codigoPostalDB)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Datos = new List<CodigosPostales>();
                respuesta.Mensaje = $"No se encuentra el codigo postal";
                return respuesta;
            }

            var query = await (from cp in datos.CodigosPostales
                               join e in datos.Estados on cp.c_Estado equals e.c_Estado
                               where cp.c_CodigoPostal == value
                               select new { cp, e }).ToListAsync().ConfigureAwait(false);

            respuesta.Datos = query.Select(q => new CodigosPostales
            {
                c_CodigoPostal = q.cp.c_CodigoPostal,
                c_Estado = q.cp.c_Estado,
                c_Localidad = q.cp.c_Localidad,
                c_Municipio = q.cp.c_Municipio,
                Estado = new Estados
                {
                    c_Estado = q.e.c_Estado,
                    c_Pais = q.e.c_Pais,
                    Nombre = q.e.Nombre
                }
            }).ToList();

            respuesta.Datos.First().Municipio = await datos.Municipios.FirstOrDefaultAsync(l => l.c_Municipio == respuesta.Datos.First().c_Municipio && l.c_Estado == respuesta.Datos.First().c_Estado).ConfigureAwait(false);

            respuesta.Datos.First().Localidad = await datos.Localidades.FirstOrDefaultAsync(l => l.c_Localidad == respuesta.Datos.First().c_Localidad && l.c_Estado == respuesta.Datos.First().c_Estado).ConfigureAwait(false);

            respuesta.Datos.First().Colonias = await datos.Colonias.Where(c => c.c_CodigoPostal == value).ToListAsync().ConfigureAwait(false);

            return respuesta;
        }


    }
}
