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
    public class MP_Pelicula: Mapper<Pelicula>
    {
        public override Pelicula GetById(object id)
        {
            int ID = int.Parse(id.ToString());
            return GetAll().FirstOrDefault(p => p.ID.Equals(ID));
        }

        MP_Emocion mpEmocion = new MP_Emocion();

        public override Pelicula Transform(DataRow dr)
        {
            Pelicula pelicula = new Pelicula();
            pelicula.ID = int.Parse(dr["ID"].ToString());
            pelicula.Nombre = dr["NOMBRE"].ToString();
            pelicula.Descripcion = dr["DESCRIPCION"].ToString();
            pelicula.Fecha = DateTime.Parse(dr["FECHA"].ToString());
            pelicula.Genero = dr["GENERO"].ToString();
            pelicula.Director = dr["DIRECTOR"].ToString();
            pelicula.Emocion = mpEmocion.GetById(dr["ID_EMOCION"].ToString());
            pelicula.Uri = dr["URI_RELATIVO"].ToString();
            pelicula.Precio = float.Parse(dr["PRECIO"].ToString());

            return pelicula;
        }

        public override List<Pelicula> GetAll()
        {
            List<Pelicula> peliculas = new List<Pelicula>();

            access.Open();
            DataTable dt = access.Read("LISTAR_PELICULA");
            access.Close();

            foreach (DataRow dr in dt.Rows)
            {
                peliculas.Add(Transform(dr));
            }

            return peliculas;
        }

        public override int Insert(Pelicula entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@Nombre", entity.Nombre),
                access.CreateParameter("@Descripcion", entity.Descripcion),
                access.CreateParameter("@Fecha", entity.Fecha),
                access.CreateParameter("@Genero", entity.Genero),
                access.CreateParameter("@Director", entity.Director),
                access.CreateParameter("@ID_Emocion", entity.Emocion.ID),
                access.CreateParameter("@Uri_Relativo", entity.Uri),
                access.CreateParameter("@Precio", entity.Precio),
            };

            access.Open();
            int resultado = access.Write("INSERTAR_PELICULA", parameters);
            access.Close();
            return resultado;


        }

        public override int Update(Pelicula entity)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@ID", entity.ID),
                access.CreateParameter("@Nombre", entity.Nombre),
                access.CreateParameter("@Descripcion", entity.Descripcion),
                access.CreateParameter("@Fecha", entity.Fecha),
                access.CreateParameter("@Genero", entity.Genero),
                access.CreateParameter("@Director", entity.Director),
                access.CreateParameter("@ID_Emocion", entity.Emocion.ID),
                access.CreateParameter("@Uri_Relativo", entity.Uri),
                access.CreateParameter("@Precio", entity.Precio),
            };

            access.Open();
            int resultado = access.Write("MODIFICAR_PELICULA", parameters);
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
            int resultado = access.Write("ELIMINAR_PELICULA", parameters);
            access.Close();

            return resultado;
        }
    }
}
