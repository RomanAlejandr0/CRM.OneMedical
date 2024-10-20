using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRM.OneMedical.Shared;
using CRM.OneMedical.Shared.EntidadesDB;
using CRM.OneMedical.Shared.Datos;
using CRM.OneMedical.Shared.EntidadesDB.AdministracionPerfilesTareas;
using Microsoft.AspNetCore.Mvc.Formatters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryMobil.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionController : ControllerBase
    {
        private IConfiguration config;
        private AccesoDatos datos;

        public AutorizacionController(IConfiguration config, AccesoDatos datos)
        {
            this.config = config;
            this.datos = datos;
        }

        [HttpPost]
        public async Task<Respuesta<UserToken>> Post([FromBody] UserInfo usuario)
        {
            var respuesta = new Respuesta<UserToken>() { Datos = new UserToken() };
            try
            {

                var user = await datos.Tbl_Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Password == usuario.Password.Trim() && u.EMail == usuario.Email).ConfigureAwait(false);

                if (user != null)
                {
                    var UserName = user.Nombre.ToLower();
                    var permisosPerfil = await (from pt in datos.Tbl_Adm_Perfil_Tarea
                                                join t in datos.Tbl_Adm_Tarea on pt.TareaId equals t.TareaId
                                                where pt.PerfilId == user.Perfil
                                                select t).ToListAsync().ConfigureAwait(false);

                    var identity = new IdentityUser { UserName = UserName };

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(1)
                    };

                    authProperties.Items.Add("IdAuthUsuario", user.UsuarioId.ToString());

                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Nombre.ToUpper().Trim()) };

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")), authProperties).ConfigureAwait(true);

                    respuesta.Datos = BuildToken(user, permisosPerfil);
                    respuesta.Estatus = EstadosDeRespuesta.Correcto;
                }
                else
                {
                    respuesta.Estatus = EstadosDeRespuesta.NoProceso;
                    respuesta.Mensaje = "Error de usuario y contraseña";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = "Error al authenticar";
            }
            return respuesta;
        }


        private UserToken BuildToken(Usuario usuario, List<Tbl_Adm_Tarea> permisos)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Nombre),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.EMail),
                new Claim("AuthId", usuario.UsuarioId.ToString())
            };

            foreach (var permiso in permisos)
            {
                claims.Add(new Claim(ClaimTypes.Role, permiso.Descripcion));
            }
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expira = DateTime.UtcNow.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: null,
                audience: null,
                claims: claims,
                expires: null,
                signingCredentials: creds
            );
            return new UserToken { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = expira };
        }




        //[HttpPost]
        //[Route("{action}")]
        //public async Task<Respuesta<UserToken>> ValidarContraseña([FromBody] UserInfo usuario)
        //{
        //    var respuesta = new Respuesta<UserToken>() { Datos = new UserToken() };
        //    try
        //    {
        //        var user = await datos.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.RecoverPassword == usuario.Password.Encriptar()).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            var UserName = user.Nombre.ToLower();
        //            var permisosPerfil = await (from pt in datos.Tbl_Adm_Perfil_Tarea
        //                                        join t in datos.Tbl_Adm_Tarea on pt.TareaId equals t.TareaId
        //                                        where pt.PerfilId == user.PerfilId
        //                                        select t).ToListAsync().ConfigureAwait(false);

        //            var identity = new IdentityUser { UserName = UserName };

        //            var authProperties = new AuthenticationProperties
        //            {
        //                IsPersistent = true,
        //                ExpiresUtc = DateTime.UtcNow.AddDays(1)
        //            };

        //            authProperties.Items.Add("IdAuthUsuario", user.UsuarioId.ToString());

        //            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Nombre.ToUpper().Trim()) };

        //            respuesta.Datos = BuildToken(user, permisosPerfil);
        //            respuesta.Estado = EstadosDeRespuesta.Correcto;
        //            user.RecoverPassword = null;
        //            user.Password = user.Telefono.Trim().Encriptar();
        //            user.EsCambiarPassword = 0;
        //            datos.Entry(user).State = EntityState.Modified;
        //            await datos.SaveChangesAsync(true);
        //        }
        //        else
        //        {
        //            respuesta.Mensaje = "Error Contraseña Incorrecta";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta.Mensaje = "Error al authenticar";
        //    }
        //    return respuesta;
        //}










        // POST api/<AutorizacionController>
        //[HttpPost]
        //public async Task<Respuesta<UserToken>> Post([FromBody] UserInfo usuario)
        //{
        //    var respuesta = new Respuesta<UserToken>() { Datos = new UserToken() };
        //    try
        //    {
        //        //usuario.Usuario = usuario.Usuario.ToLower();
        //        var user = await datos.Tbl_Adm_Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Password == usuario.Password.Trim().Encriptar() && u.Telefono == usuario.Telefono).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            var UserName = user.Nombre.ToLower();
        //            var permisosPerfil = await (from pt in datos.Tbl_Adm_Perfil_Tarea
        //                                  join t in datos.Tbl_Adm_Tarea on pt.TareaId equals t.TareaId
        //                                  where pt.PerfilId == user.PerfilId
        //                                  select t).ToListAsync().ConfigureAwait(false);

        //            var identity = new IdentityUser { UserName = UserName };

        //            var authProperties = new AuthenticationProperties
        //            {
        //                IsPersistent = true,
        //                ExpiresUtc = DateTime.UtcNow.AddDays(1)
        //            };

        //            authProperties.Items.Add("IdAuthUsuario", user.UsuarioId.ToString());

        //            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Nombre.ToUpper().Trim()) };

        //            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")), authProperties).ConfigureAwait(true);

        //            respuesta.Datos = BuildToken(user, permisosPerfil);
        //            respuesta.Estado =  EstadosDeRespuesta.Correcto;
        //        }
        //        else
        //        {
        //            respuesta.Estado = EstadosDeRespuesta.NoProceso;
        //            respuesta.Mensaje = "Error de usuario y contraseña";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta.Mensaje = "Error al authenticar";
        //    }
        //    return respuesta;
        //}





        //private UserToken BuildToken(Tbl_Adm_Usuario usuario, List<Tbl_Adm_Tarea> permisos)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Nombre),
        //        new Claim(ClaimTypes.Name, usuario.Nombre),
        //        new Claim(ClaimTypes.Email, usuario.EMail),
        //        new Claim("AuthId", usuario.UsuarioId.ToString()),
        //    };

        //    foreach (var permiso in permisos)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, permiso.Descripcion));
        //    }
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expira = DateTime.UtcNow.AddDays(1);

        //    JwtSecurityToken token = new JwtSecurityToken
        //    (
        //        issuer: null,
        //        audience: null,
        //        claims: claims,
        //        expires: null,
        //        signingCredentials: creds

        //    );
        //    return new UserToken { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = expira };
        //}





        //[HttpPost]
        //[Route("{action}")]
        //public async Task<Respuesta<UserToken>> ValidarContraseña([FromBody] UserInfo usuario)
        //{
        //    var respuesta = new Respuesta<UserToken>() { Datos = new UserToken() };
        //    try
        //    {
        //        var user = await datos.Tbl_Adm_Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.RecoverPassword == usuario.Password.Encriptar()).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            var UserName = user.Nombre.ToLower();
        //            var permisosPerfil = await (from pt in datos.Tbl_Adm_Perfil_Tarea
        //                                        join t in datos.Tbl_Adm_Tarea on pt.TareaId equals t.TareaId
        //                                        where pt.PerfilId == user.PerfilId
        //                                        select t).ToListAsync().ConfigureAwait(false);

        //            var identity = new IdentityUser { UserName = UserName };

        //            var authProperties = new AuthenticationProperties
        //            {
        //                IsPersistent = true,
        //                ExpiresUtc = DateTime.UtcNow.AddDays(1)
        //            };

        //            authProperties.Items.Add("IdAuthUsuario", user.UsuarioId.ToString());

        //            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Nombre.ToUpper().Trim()) };

        //            respuesta.Datos = BuildToken(user, permisosPerfil);
        //            respuesta.Estado = EstadosDeRespuesta.Correcto;
        //            user.RecoverPassword = null;
        //            user.Password = user.Telefono.Trim().Encriptar();
        //            user.EsCambiarPassword = 0;
        //            datos.Entry(user).State = EntityState.Modified;
        //            await datos.SaveChangesAsync(true);
        //        }
        //        else
        //        {
        //            respuesta.Mensaje = "Error Contraseña Incorrecta";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        respuesta.Mensaje = "Error al authenticar";
        //    }
        //    return respuesta;
        //}

    }
}
