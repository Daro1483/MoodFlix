using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;

namespace DAL
{
    public class MP_UsuarioRol : Mapper<UsuarioRol>
    {
        public override UsuarioRol GetById(object id)
        {
            int idUsuario = Convert.ToInt32(id);

            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        access.CreateParameter("@IDUSUARIO", idUsuario)
    };

            access.Open();
            DataTable dt = access.Read("OBTENER_USUARIO_ROL", parameters);
            access.Close();

            if (dt.Rows.Count == 0)
                return null;

            return Transform(dt.Rows[0]);
        }


        public override UsuarioRol Transform(DataRow dr)
        {
            UsuarioRol ur = new UsuarioRol();
            ur.IdUsuario = int.Parse(dr["IDUSUARIO"].ToString());
            ur.IdRol = int.Parse(dr["IDROL"].ToString());
            return ur;
        }

        public List<UsuarioRol> ObtenerUsuariosPorRol(int idRol)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        access.CreateParameter("@IDROL", idRol)
    };

            access.Open();
            DataTable dt = access.Read("LISTAR_USUARIOS_CON_ROL", parameters);
            access.Close();

            return dt.Rows.Count == 0 ?
                   new List<UsuarioRol>() :
                   dt.Rows.Cast<DataRow>().Select(Transform).ToList();
        }

        public override List<UsuarioRol> GetAll()
        {
            List<UsuarioRol> lista = new List<UsuarioRol>();

            access.Open();
            DataTable dt = access.Read("LISTAR_USUARIO_ROL");
            access.Close();

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(Transform(dr));
            }

            return lista;
        }

        public override int Insert(UsuarioRol entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", entity.IdUsuario),
                access.CreateParameter("@IDROL", entity.IdRol)
            };

            access.Open();
            // No retorna ID, así que usamos Write
            int result = access.Write("INSERTAR_USUARIO_ROL", parameters);
            access.Close();

            return result; // puede ser filas afectadas
        }

        public override int Update(UsuarioRol entity)
        {
            // Generalmente no se actualiza esta tabla, pero igual lo dejo preparado
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", entity.IdUsuario),
                access.CreateParameter("@IDROL", entity.IdRol)
            };

            access.Open();
            int result = access.Write("ACTUALIZAR_USUARIO_ROL", parameters);
            access.Close();

            return result;
        }

        public override int Delete(int id)
        {
            // Se borra por IdUsuario
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", id)
            };

            access.Open();
            int result = access.Write("ELIMINAR_USUARIO_ROL", parameters);
            access.Close();

            return result;
        }

        public void Delete(int idUsuario, int idRol)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        access.CreateParameter("@IDUSUARIO", idUsuario),
        access.CreateParameter("@IDROL", idRol)
    };

            access.Open();
            access.Write("ELIMINAR_USUARIO_ROL_PUNTUAL", parameters);  // ✔ CORRECTO
            access.Close();
        }

        public List<Rol> ObtenerRolesDeUsuario(int idUsuario)
        {
            List<Rol> roles = new List<Rol>();

            access.Open();
            DataTable dt = access.Read("LISTAR_ROLES_DE_USUARIO",
                new List<SqlParameter> {
            access.CreateParameter("@IDUSUARIO", idUsuario)
                });
            access.Close();

            foreach (DataRow dr in dt.Rows)
                roles.Add(new Rol
                {
                    IdRol = int.Parse(dr["IDROL"].ToString()),
                    Nombre = dr["NOMBRE"].ToString(),
                    Tipo = dr["TIPO"].ToString()
                });

            return roles;
        }



    }
}
