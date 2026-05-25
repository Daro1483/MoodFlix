using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;
using Services;

namespace DAL
{
    public class MP_Bitacora : Mapper<Bitacora>
    {
        private readonly MP_Usuario _mpUsuario = new MP_Usuario();

        // ============================================================
        // TRANSFORM
        // ============================================================
        public override Bitacora Transform(DataRow dr)
        {
            Bitacora bit = new Bitacora();

            bit.ID = Convert.ToInt32(dr["ID"]);
            bit.Fecha = Convert.ToDateTime(dr["FECHA"]);

            // Modulo
            try
            {
                bit.Modulo = (TipoModulo)Enum.Parse(typeof(TipoModulo), dr["MODULO"].ToString());
            }
            catch
            {
                bit.Modulo = TipoModulo.Desconocido;
            }

            // Operación
            try
            {
                bit.Operacion = (TipoOperacion)Enum.Parse(typeof(TipoOperacion), dr["OPERACION"].ToString());
            }
            catch
            {
                bit.Operacion = TipoOperacion.Desconocida;
            }

            // Usuario
            bit.User = new Usuario();
            bit.User.ID = Convert.ToInt32(dr["ID_USUARIO"]);

            return bit;
        }

        public override Bitacora GetById(object id)
        {
            throw new NotImplementedException();
        }

        // ============================================================
        // LISTAR TODO
        // ============================================================
        public override List<Bitacora> GetAll()
        {
            access.Open();
            DataTable dt = access.Read("LISTAR_BITACORA");
            access.Close();

            List<Usuario> usuarios = _mpUsuario.GetAll();
            List<Bitacora> lista = new List<Bitacora>();

            foreach (DataRow dr in dt.Rows)
            {
                Bitacora b = Transform(dr);
                b.User = usuarios.FirstOrDefault(u => u.ID == b.User.ID);
                lista.Add(b);
            }

            return lista;
        }

        // ============================================================
        // FILTRO SIMPLE POR FECHAS
        // ============================================================
        public List<Bitacora> FiltrarBitacora(DateTime fechaInicio, DateTime fechaFin)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@FechaInicio", fechaInicio),
                access.CreateParameter("@FechaFin", fechaFin)
            };

            access.Open();
            DataTable dt = access.Read("FILTRAR_BITACORA_FECHAS", parameters);
            access.Close();

            List<Usuario> usuarios = _mpUsuario.GetAll();
            List<Bitacora> lista = new List<Bitacora>();

            foreach (DataRow dr in dt.Rows)
            {
                Bitacora b = Transform(dr);
                b.User = usuarios.FirstOrDefault(u => u.ID == b.User.ID);
                lista.Add(b);
            }

            return lista;
        }

        // ============================================================
        // FILTRO AVANZADO (fecha, email, modulo)
        // ============================================================
        public List<Bitacora> FiltrarAvanzado(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string email,
            string modulo)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                access.CreateParameter("@FechaInicio", fechaInicio),
                access.CreateParameter("@FechaFin", fechaFin),
                access.CreateParameter("@Email", string.IsNullOrWhiteSpace(email) ? null : email),
                access.CreateParameter("@Modulo", string.IsNullOrWhiteSpace(modulo) ? null : modulo)
            };

            access.Open();
            DataTable dt = access.Read("FILTRAR_BITACORA_AVANZADO", parametros);
            access.Close();

            List<Usuario> usuarios = _mpUsuario.GetAll();
            List<Bitacora> lista = new List<Bitacora>();

            foreach (DataRow dr in dt.Rows)
            {
                Bitacora b = Transform(dr);
                b.User = usuarios.FirstOrDefault(u => u.ID == b.User.ID);
                lista.Add(b);
            }

            return lista;
        }

        // ============================================================
        // INSERTAR
        // ============================================================
        public override int Insert(Bitacora entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@fecha", entity.Fecha),
                access.CreateParameter("@id_usuario", entity.User.ID),
                access.CreateParameter("@modulo", entity.Modulo.ToString()),
                access.CreateParameter("@operacion", entity.Operacion.ToString())
            };

            access.Open();
            int filas = access.Write("INSERTAR_BITACORA", parameters);
            access.Close();

            return filas;
        }

        public override int Update(Bitacora entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
