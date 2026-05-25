using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class Libro
    {
        private MP_Libro mpLibro = new MP_Libro();

        public List<BE.Libro> Listar()
        {
            return mpLibro.GetAll();
        }

        public BE.Libro ObtenerPorId(int id)
        {
            return mpLibro.GetById(id);
        }


        public string Concatenar(BE.Libro libro)
        {
            return string.Concat(libro.ID + libro.Nombre + libro.Descripcion + libro.Fecha + libro.Autor +
                                 libro.Editorial + libro.Emocion.ID + libro.Uri + libro.Precio);
        }

        public void Insertar(BE.Libro libro)
        {
            mpLibro.Insert(libro);
        }

        public void Actualizar(BE.Libro libro)
        {
            mpLibro.Update(libro);
        }

        public void Eliminar(int id)
        {
            mpLibro.Delete(id);
        }
    }
}
