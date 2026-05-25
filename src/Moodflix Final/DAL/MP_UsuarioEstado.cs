using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DAL;
using BE;

namespace DAL
{
    public class MP_UsuarioEstado : Mapper<UsuarioEstadoDTO>
    {
        // ============================================================
        // GET ALL  (usa el SP LISTAR_USUARIOS_ESTADO)
        // ============================================================
        public override List<UsuarioEstadoDTO> GetAll()
        {
            List<UsuarioEstadoDTO> lista = new List<UsuarioEstadoDTO>();

            access.Open();
            DataTable dt = access.Read("LISTAR_USUARIOS_ESTADO");
            access.Close();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(Transform(row));
            }

            return lista;
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public override UsuarioEstadoDTO GetById(object id)
        {
            UsuarioEstadoDTO dto = null;

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", (int)id)
            };

            access.Open();
            DataTable dt = access.Read("OBTENER_USUARIO_ESTADO", parameters);
            access.Close();

            if (dt.Rows.Count > 0)
                dto = Transform(dt.Rows[0]);

            return dto;
        }

        // ============================================================
        // INSERT — se usa cuando se crea un usuario nuevo
        // ============================================================
        public override int Insert(UsuarioEstadoDTO entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", entity.IdUsuario),
                access.CreateParameter("@INTENTOS", entity.Intentos),
                access.CreateParameter("@BLOQUEADO", entity.Bloqueado)
            };

            access.Open();
            int result = access.Write("INSERTAR_USUARIO_ESTADO", parameters);
            access.Close();

            return result;
        }

        // ============================================================
        // UPDATE — actualizar intentos y/o bloqueo
        // ============================================================
        public override int Update(UsuarioEstadoDTO entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", entity.IdUsuario),
                access.CreateParameter("@INTENTOS", entity.Intentos),
                access.CreateParameter("@BLOQUEADO", entity.Bloqueado)
            };

            access.Open();
            int result = access.Write("ACTUALIZAR_USUARIO_ESTADO", parameters);
            access.Close();

            return result;
        }

        // ============================================================
        // DELETE — NO se usa, pero implementamos para cumplir la abstracción
        // ============================================================
        public override int Delete(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDUSUARIO", id)
            };

            access.Open();
            int result = access.Write("ELIMINAR_USUARIO_ESTADO", parameters);
            access.Close();

            return result;
        }

        // ============================================================
        // TRANSFORM — DataRow -> DTO
        // ============================================================
        public override UsuarioEstadoDTO Transform(DataRow dr)
        {
            UsuarioEstadoDTO dto = new UsuarioEstadoDTO();

            // ======== SP LISTAR_USUARIOS_ESTADO ========
            if (dr.Table.Columns.Contains("USERNAME"))
            {
                dto.IdUsuario = Convert.ToInt32(dr["IDUSUARIO"]);
                dto.Username = dr["USERNAME"].ToString();
                dto.Email = dr["EMAIL"].ToString();
                dto.Intentos = Convert.ToInt32(dr["INTENTOS"]);
                dto.Bloqueado = Convert.ToBoolean(dr["BLOQUEADO"]);
                return dto;
            }

            // ======== SP OBTENER_USUARIO_ESTADO ========
            if (dr.Table.Columns.Contains("IDUSUARIO") &&
                !dr.Table.Columns.Contains("USERNAME"))
            {
                dto.IdUsuario = Convert.ToInt32(dr["IDUSUARIO"]);
                dto.Username = string.Empty; // No viene en este SP
                dto.Email = string.Empty;    // No viene en este SP
                dto.Intentos = Convert.ToInt32(dr["INTENTOS"]);
                dto.Bloqueado = Convert.ToBoolean(dr["BLOQUEADO"]);
                return dto;
            }

            throw new Exception("Estructura inesperada en UsuarioEstadoDTO.Transform");
        }






    }
}
