using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class MP_Libro: Mapper<Libro>
    {
        public override Libro GetById(object id)
        {
            int ID = int.Parse(id.ToString());
            return GetAll().FirstOrDefault(l => l.ID.Equals(ID));
        }

        private MP_Emocion mpEmocion = new MP_Emocion();

        public override Libro Transform(DataRow dr)
        {
            Libro libro = new Libro();
            libro.ID = int.Parse(dr["ID"].ToString());
            libro.Nombre = dr["NOMBRE"].ToString();
            libro.Descripcion = dr["DESCRIPCION"].ToString();
            libro.Fecha = DateTime.Parse(dr["FECHA"].ToString());
            libro.Autor = dr["AUTOR"].ToString();
            libro.Editorial = dr["EDITORIAL"].ToString();
            libro.Emocion = mpEmocion.GetById(dr["ID_EMOCION"].ToString());
            libro.Uri = dr["URI_RELATIVO"].ToString();
            libro.Precio = float.Parse(dr["PRECIO"].ToString());

            return libro;


        }

        public override List<Libro> GetAll()
        {
            List<Libro> libros = new List<Libro>();

            access.Open();
            DataTable dt = access.Read("LISTAR_LIBRO");
            access.Close();

            foreach (DataRow dr in dt.Rows)
            {
                libros.Add(Transform(dr));
            }

            return libros;
        }

        public override int Insert(Libro entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@Nombre", entity.Nombre),
                access.CreateParameter("@Descripcion", entity.Descripcion),
                access.CreateParameter("@Fecha", entity.Fecha),
                access.CreateParameter("@Autor", entity.Autor),
                access.CreateParameter("@Editorial", entity.Editorial),
                access.CreateParameter("@ID_Emocion", entity.Emocion.ID),
                access.CreateParameter("@Uri_Relativo", entity.Uri),
                access.CreateParameter("@Precio", entity.Precio),
            };

            access.Open();
            int resultado = access.Write("INSERTAR_LIBRO", parameters);
            access.Close();
            return resultado;
        }

        public override int Update(Libro entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@ID", entity.ID),
                access.CreateParameter("@Nombre", entity.Nombre),
                access.CreateParameter("@Descripcion", entity.Descripcion),
                access.CreateParameter("@Fecha", entity.Fecha),
                access.CreateParameter("@Autor", entity.Autor),
                access.CreateParameter("@Editorial", entity.Editorial),
                access.CreateParameter("@ID_Emocion", entity.Emocion.ID),
                access.CreateParameter("@Uri_Relativo", entity.Uri),
                access.CreateParameter("@Precio", entity.Precio),
            };

            access.Open();
            int resultado = access.Write("MODIFICAR_LIBRO", parameters);
            access.Close();
            return resultado;
        }

        public override int Delete(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@ID", id),

            };


            access.Open();
            int resultado = access.Write("ELIMINAR_LIBRO", parameters);
            access.Close();

            return resultado;
        }
    }
}
