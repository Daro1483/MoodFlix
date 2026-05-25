using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class Pelicula
    {
        MP_Pelicula mpPelicula = new MP_Pelicula();
        public List<BE.Pelicula> Listar()
        {
            return mpPelicula.GetAll();
        }

        public BE.Pelicula ObtenerPorId(int id)
        {
            return mpPelicula.GetById(id);
        }

        public string Concatenar(BE.Pelicula pelicula)
        {
            return string.Concat(pelicula.ID + pelicula.Nombre + pelicula.Descripcion + pelicula.Fecha +
                                 pelicula.Genero + pelicula.Director + pelicula.Emocion.ID + pelicula.Uri +
                                 pelicula.Precio);
        }

        public void Insertar(BE.Pelicula pelicula)
        {
            mpPelicula.Insert(pelicula);
        }

        public void Actualizar(BE.Pelicula pelicula)
        {
            mpPelicula.Update(pelicula);
        }

        public void Eliminar(int id)
        {
            mpPelicula.Delete(id);
        }
    }
}
