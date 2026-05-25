using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;

namespace DAL
{
    public class MP_RolHijo : Mapper<RolHijo>
    {
        public override RolHijo GetById(object id)
        {
            // En este caso, id será el IdRolPadre
            int idRolPadre = Convert.ToInt32(id);

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDROLPADRE", idRolPadre)
            };

            access.Open();
            DataTable dt = access.Read("OBTENER_ROLH",
                                       parameters);
            access.Close();

            if (dt.Rows.Count == 0)
                return null;

            return Transform(dt.Rows[0]);
        }

        public override RolHijo Transform(DataRow dr)
        {
            RolHijo rh = new RolHijo();

            rh.IdRolPadre = int.Parse(dr["IDROLPADRE"].ToString());
            rh.IdRolHijo = int.Parse(dr["IDROLHIJO"].ToString());

            return rh;
        }

        public override List<RolHijo> GetAll()
        {
            List<RolHijo> lista = new List<RolHijo>();

            access.Open();
            DataTable dt = access.Read("LISTAR_ROLHIJO");
            access.Close();

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(Transform(dr));
            }

            return lista;
        }

        public override int Insert(RolHijo entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDROLPADRE", entity.IdRolPadre),
                access.CreateParameter("@IDROLHIJO", entity.IdRolHijo)
            };

            access.Open();
            int result = access.Write("INSERTAR_ROLHIJO", parameters);
            access.Close();

            return result; // filas afectadas
        }

        public override int Update(RolHijo entity)
        {
            // Normalmente esta tabla NO se actualiza.
            // Pero igual dejamos preparado el SP por consistencia.

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDROLPADRE", entity.IdRolPadre),
                access.CreateParameter("@IDROLHIJO", entity.IdRolHijo)
            };

            access.Open();
            int result = access.Write("ACTUALIZAR_ROLHIJO", parameters);
            access.Close();

            return result;
        }

        public override int Delete(int id)
        {
            // Se elimina por IdRolPadre (full cascade o parcial depende del SP)

            int idRolPadre = Convert.ToInt32(id);

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@IDROLPADRE", idRolPadre)
            };

            access.Open();
            int result = access.Write("ELIMINAR_ROLHIJO", parameters);
            access.Close();

            return result;
        }

        public int DeletePuntual(int idRolPadre, int idRolHijo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
        access.CreateParameter("@IDROLPADRE", idRolPadre),
        access.CreateParameter("@IDROLHIJO", idRolHijo)
            };

            access.Open();
            int result = access.Write("ELIMINAR_ROLH_PUNTUAL", parameters);
            access.Close();

            return result;
        }

        public void EliminarHijosDeFamilia(int idPadre)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        access.CreateParameter("@IDPADRE", idPadre)
    };

            access.Open();
            access.Write("ELIMINAR_HIJOS_DE_FAMILIA", parameters);
            access.Close();
        }




        public List<int> ObtenerRolesHijoRecursivo(int idRolPadre)
        {
            List<int> resultado = new List<int>();

            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        access.CreateParameter("@IDROLPADRE", idRolPadre)
    };

            access.Open();   // ← FALTA EN TU CÓDIGO
            DataTable dt = access.Read("OBTENER_ROLS_HIJOS", parameters);
            access.Close();  // ← FALTA EN TU CÓDIGO

            foreach (DataRow dr in dt.Rows)
            {
                int idHijo = int.Parse(dr["IDROLHIJO"].ToString());
                resultado.Add(idHijo);

                // Recursión → SIEMPRE correcto
                resultado.AddRange(ObtenerRolesHijoRecursivo(idHijo));
            }

            return resultado.Distinct().ToList();
        }




    }
}
