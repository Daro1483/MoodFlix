using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace BLL
{
    public class RolBLL
    {
        private readonly MP_Rol mapper = new MP_Rol();
        

        /// <summary>
        /// Obtiene todos los roles registrados (patentes y familias).
        /// </summary>
        public List<Rol> GetAll()
        {
            return mapper.GetAll();
        }           

        public int CrearFamilia(string nombre)
        {
            Rol nuevaFamilia = new Rol()
            {
                Nombre = nombre,
                Tipo = "FAMILIA"
            };

            return mapper.Insert(nuevaFamilia);
        }

        public List<Rol> GetFamilias()
        {
            return mapper
                .GetAll()
                .Where(r => r.Tipo != null && r.Tipo.ToUpper() == "FAMILIA")
                .ToList();
        }


        public List<Rol> GetPatentes()
        {
            return mapper
                .GetAll()
                .Where(r => r.Tipo != null && r.Tipo.ToUpper() == "PATENTE")
                .ToList();
        }

        public Rol GetById(int id)
        {
            return mapper.GetById(id);
        }

        public void EliminarFamilia(int idFamilia)
        {
            mapper.EliminarFamilia(idFamilia);
        }


    }
}
