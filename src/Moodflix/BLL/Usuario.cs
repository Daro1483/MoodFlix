using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;
using Services;

namespace BLL
{
    public class Usuario
    {
        private readonly MP_Usuario _mpUsuario = new MP_Usuario();

        // ============================================================
        // VALIDACIÓN DE LOGIN
        // ============================================================
        public bool ValidarUsuario(BE.Usuario usuarioLogin)
        {
            var user = GetUser(usuarioLogin.Email);

            if (user == null)
                throw new LoginException(LoginResult.InvalidEmail);

            if (CryptoManager.Compare(usuarioLogin.Password, user.Password))
                return true;

            throw new LoginException(LoginResult.InvalidPassword);
        }

        public int GetIdByEmail(string email)
        {
            var user = GetUser(email);
            return user != null ? user.ID : 0;
        }

        // ============================================================
        // OBTENER USUARIO COMPLETO (CON ROLES Y PATENTES)
        // ============================================================
        public BE.Usuario GetUser(string email)
        {
            var usuario = _mpUsuario.GetAll().FirstOrDefault(u => u.Email.Equals(email));

            if (usuario == null)
                return null;

            var mpUsuarioRol = new MP_UsuarioRol();
            var mpRol = new MP_Rol();
            var mpRolHijo = new MP_RolHijo();

            // Roles directos
            List<Rol> rolesDirectos = mpUsuarioRol.ObtenerRolesDeUsuario(usuario.ID);
            List<Rol> rolesTotales = new List<Rol>(rolesDirectos);

            // Roles hijos recursivos
            foreach (var rol in rolesDirectos)
            {
                var hijos = mpRolHijo.ObtenerRolesHijoRecursivo(rol.IdRol);
                foreach (var idHijo in hijos)
                {
                    var rolHijo = mpRol.GetById(idHijo);
                    if (rolHijo != null)
                        rolesTotales.Add(rolHijo);
                }
            }

            usuario.Roles = rolesTotales.Distinct().ToList();

            // Patentes (solo roles tipo PATENTE)
            usuario.Patentes = usuario.Roles
                .Where(r => r.Tipo.Equals("PATENTE", StringComparison.OrdinalIgnoreCase))
                .Select(r => new BE.Patente
                {
                    Nombre = r.Nombre
                })
                .ToList();

            return usuario;
        }

        public BE.Usuario GetUserByUsername(string username)
        {
            return _mpUsuario.GetAll()
                .FirstOrDefault(u => u.Username.Equals(username));
        }

        // ============================================================
        // CRUD BÁSICO
        // ============================================================
        public List<BE.Usuario> Listar()
        {
            return _mpUsuario.GetAll();
        }

        public int Insertar(BE.Usuario usuario)
        {
            return _mpUsuario.Insert(usuario);
        }

        // ============================================================
        // VALIDACIÓN DE PERMISOS (PATENTES)
        // ============================================================
        public bool UsuarioTienePatente(BE.Usuario usuario, string nombrePatente)
        {
            if (usuario == null || usuario.Patentes == null)
                return false;

            if (string.IsNullOrWhiteSpace(nombrePatente))
                return false;

            return usuario.Patentes.Any(p =>
                p.Nombre != null &&
                p.Nombre.Trim().Equals(nombrePatente.Trim(), StringComparison.OrdinalIgnoreCase)
            );
        }

        // ============================================================
        // UTILIDADES
        // ============================================================
        public string Concatenar(BE.Usuario usuario)
        {
            return $"{usuario.ID}{usuario.Email}{usuario.Password}{usuario.Username}";
        }
    }
}
