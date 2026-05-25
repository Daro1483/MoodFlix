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
    public class MP_Emocion: Mapper<Emocion>
    {
        public override Emocion GetById(object id)
        {
            int ID = int.Parse(id.ToString());
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                access.CreateParameter("@Id", ID),
            };

            access.Open();
            DataTable dt = access.Read("LISTAR_EMOCION_POR_ID", parameters);
            access.Close();
            DataRow dr = dt.Rows[0];

            Emocion emocion = Transform(dr);

            return emocion;

        }

        public override Emocion Transform(DataRow dr)
        {
            Emocion emocion = new Emocion();
            emocion.ID = int.Parse(dr["ID"].ToString());

            try
            {
                emocion.TipoEmocion = (TipoEmocion)Enum.Parse(typeof(TipoEmocion), dr["NOMBRE"].ToString());
            }
            catch
            {
                emocion.TipoEmocion = TipoEmocion.Desconocida;
            }

            
            emocion.Uri = dr["URI_RELATIVO"].ToString();

            return emocion;
        }

        public override List<Emocion> GetAll()
        {
            List<Emocion> emociones = new List<Emocion>();

            access.Open();
            DataTable dt = access.Read("LISTAR_EMOCION");
            access.Close();

            foreach (DataRow dr in dt.Rows)
            {
                emociones.Add(Transform(dr));
            }

            return emociones;

        }

        public override int Insert(Emocion entity)
        {
            throw new NotImplementedException();
        }

        public override int Update(Emocion entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
