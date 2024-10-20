using CRM.OneMedical.Client.Manager;
using CRM.OneMedical.Shared;
using CRM.OneMedical.Shared.Datos;
using CRM.OneMedical.Shared.EntidadesDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.OneMedical.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminUsuariosController : ControllerBase
    {
        private readonly AccesoDatos db;

        public AdminUsuariosController(AccesoDatos db)
        {
            this.db = db;
        }


        [HttpGet("{usuarioId}")]
        public async Task<Respuesta<Usuario>> Get(long usuarioId)
        {
            var respuesta = new Respuesta<Usuario> { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                var usuario = new Usuario();

                usuario = await db.Tbl_Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

                respuesta.Datos = usuario;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al consultar el usuario";
            }

            return respuesta;
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<Respuesta<long>> Guardar([FromBody] Usuario usuario)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto, Mensaje = "Guardado Correctamente" };

            if (usuario.UsuarioId == 0)
            {
                usuario.Fecha = DateTime.Now;

                respuesta = await GuardarUsuario(usuario);

            }
            else
            {
                respuesta = await EditarUsuario(usuario);
            }

            return respuesta;
        }

        public async Task<Respuesta<long>> GuardarUsuario([FromBody] Usuario usuario)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto, Mensaje = "Guardado Correctamente" };

            try
            {
                //usuario.Nombre = usuario.Nombre.ToUpper().Trim();
                //usuario.Email = !string.IsNullOrEmpty(usuario.Email) ? usuario.Email.Trim().ToLower() : "mail@mail.com";
                //usuario.Telefono = usuario.Telefono.Trim();
                //usuario.PasswordLogin = usuario.PasswordLogin.Encriptar();
                await db.Tbl_Usuarios.AddAsync(usuario);
                await db.SaveChangesAsync(true);

                usuario.InformacionAdicionalDelPaciente.UsuarioId = usuario.UsuarioId;
                
                await db.Tbl_InformacionAdicionalDelPaciente.AddAsync(usuario.InformacionAdicionalDelPaciente);
                await db.SaveChangesAsync(true);     
                
                respuesta.Datos = usuario.UsuarioId;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Ocurrio un error al guardar la empresa: {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
            }
            return respuesta;
        }

        public async Task<Respuesta<long>> EditarUsuario([FromBody] Usuario usuario)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                db.Attach(usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                respuesta.Datos = usuario.UsuarioId;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al guardar al actualizar el usuario {usuario.Nombre}";
            }

            return respuesta;
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<Respuesta<long>> EliminarUsuario([FromBody] Usuario usuario)
        {
            var respuesta = new Respuesta<long> { Estatus = EstadosDeRespuesta.Correcto };

            try
            {
                //context.Remove(new Provedor { Id = id });
                db.Attach(usuario).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                respuesta.Estatus = EstadosDeRespuesta.Error;
                respuesta.Mensaje = $"Error al guardar al eliminar el usuario";
            }

            return respuesta;
        }
    }
}
