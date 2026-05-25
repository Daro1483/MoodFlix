using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class Emocion
    {
        private MP_Emocion mpEmocion = new MP_Emocion();

        public List<BE.Emocion> Listar()
        {
            return mpEmocion.GetAll();
        }

        public string Concatenar(BE.Emocion emocion)
        {
            return string.Concat(emocion.ID + emocion.TipoEmocion.ToString() + emocion.Uri);
        }


    }
}
