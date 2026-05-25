using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class MP_Rol : Mapper<Rol>
    {
        public override Rol Transform(DataRow dr)
        {
            return new Rol
            {
                IdRol = Convert.ToInt32(dr["IDROL"]),
                Nombre = dr["NOMBRE"].ToString(),
                Tipo = dr["TIPO"].ToString()
            };
        }

        public override List<Rol> GetAll()
        {
            List<Rol> lista = new List<Rol>();

            access.Open();
            DataTable dt = access.Read("LISTAR_ROL");
            access.Close();

            foreach (DataRow dr in dt.Rows)
                lista.Add(Transform(dr));

            return lista;
        }


        public override Rol GetById(object id)
        {
            access.Open();

            DataTable dt = access.Read("OBTENER_ROL",
                new List<SqlParameter>
                {
            new SqlParameter("@ID", id)
                }
            );

            access.Close();

            if (dt.Rows.Count == 0)
                return null;

            return Transform(dt.Rows[0]);
        }

        public void EliminarFamilia(int idRol)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
    {
        access.CreateParameter("@IDROL", idRol)
    };

            access.Open();
            access.Write("ELIMINAR_FAMILIA", parameters);
            access.Close();
        }

        public override int Insert(Rol entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@NOMBRE", entity.Nombre),
                access.CreateParameter("@TIPO", entity.Tipo),
                new SqlParameter("@IDROL", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            access.Open();
            access.Write("INSERTAR_ROL", parameters);
            access.Close();

            // devolvemos el ID generado
            return Convert.ToInt32(parameters[2].Value);
        }


        public override int Update(Rol entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
