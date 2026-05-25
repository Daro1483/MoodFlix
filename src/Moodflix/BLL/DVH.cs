using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using Services;

namespace BLL
{
    public class DVH
    {
        MP_Dvh mpDvh = new MP_Dvh();
        Usuario bllUsuario = new Usuario();
        Emocion bllEmocion = new Emocion();
        Pelicula bllPelicula = new Pelicula();
        Libro bllLibro = new Libro();
        Bitacora bllBitacora = new Bitacora();



        public List<Services.DVH> Listar()
        {
            return mpDvh.GetAll();
        }

        public int Insertar(Services.DVH dvh)
        {
            return mpDvh.Insert(dvh);
        }

        public int Actualizar(Services.DVH dvh)
        {
            return mpDvh.Update(dvh);
        }

        public void BorrarRegistros()
        {
            mpDvh.DeleteAll();
        }


        public string ObtenerDV(string cadena)
        {
            return CryptoManager.Hash(cadena);
        }

        

        public void Recalcular(List<Services.DVH> dvhs, List<BE.Usuario> usuarios)
        {
            for (int i = 0; i < usuarios.Count; i++)
            {
                string cadena = bllUsuario.Concatenar(usuarios[i]);
                Services.DVH dvh = new Services.DVH();
                dvh.Tabla = "USUARIO";
                dvh.Registro = usuarios[i].ID;
                dvh.DV = ObtenerDV(cadena);

                var existeDvh = dvhs.FirstOrDefault(d => d.Tabla == dvh.Tabla && d.Registro == dvh.Registro);
                if (existeDvh != null)
                {
                    Actualizar(dvh);
                }
                else
                {
                    Insertar(dvh);
                }

            }
        }
        public void Recalcular(List<Services.DVH> dvhs, List<Services.Bitacora> bitacora)
        {
            for (int i = 0; i < bitacora.Count; i++)
            {
                string cadena = bllBitacora.Concatenar(bitacora[i]);
                Services.DVH dvh = new Services.DVH();
                dvh.Tabla = "BITACORA";
                dvh.Registro = bitacora[i].ID;
                dvh.DV = ObtenerDV(cadena);

                var existeDvh = dvhs.FirstOrDefault(d => d.Tabla == dvh.Tabla && d.Registro == dvh.Registro);
                if (existeDvh != null)
                {
                    Actualizar(dvh);
                }
                else
                {
                    Insertar(dvh);
                }

            }
        }

        public void Recalcular(List<Services.DVH> dvhs, List<BE.Emocion> emociones)
        {
            for (int i = 0; i < emociones.Count; i++)
            {
                string cadena = bllEmocion.Concatenar(emociones[i]);
                Services.DVH dvh = new Services.DVH();
                dvh.Tabla = "EMOCION";
                dvh.Registro = emociones[i].ID;
                dvh.DV = ObtenerDV(cadena);

                var existeDvh = dvhs.FirstOrDefault(d => d.Tabla == dvh.Tabla && d.Registro == dvh.Registro);
                if (existeDvh != null)
                {
                    Actualizar(dvh);
                }
                else
                {
                    Insertar(dvh);
                }

            }
        }

        public void Recalcular(List<Services.DVH> dvhs, List<BE.Pelicula> peliculas)
        {
            for (int i = 0; i < peliculas.Count; i++)
            {
                string cadena = bllPelicula.Concatenar(peliculas[i]);
                Services.DVH dvh = new Services.DVH();
                dvh.Tabla = "PELICULA";
                dvh.Registro = peliculas[i].ID;
                dvh.DV = ObtenerDV(cadena);

                var existeDvh = dvhs.FirstOrDefault(d => d.Tabla == dvh.Tabla && d.Registro == dvh.Registro);
                if (existeDvh != null)
                {
                    Actualizar(dvh);
                }
                else
                {
                    Insertar(dvh);
                }

            }
        }
        public void Recalcular(List<Services.DVH> dvhs, List<BE.Libro> libros)
        {
            for (int i = 0; i < libros.Count; i++)
            {
                string cadena = bllLibro.Concatenar(libros[i]);
                Services.DVH dvh = new Services.DVH();
                dvh.Tabla = "LIBRO";
                dvh.Registro = libros[i].ID;
                dvh.DV = ObtenerDV(cadena);

                var existeDvh = dvhs.FirstOrDefault(d => d.Tabla == dvh.Tabla && d.Registro == dvh.Registro);
                if (existeDvh != null)
                {
                    Actualizar(dvh);
                }
                else
                {
                    Insertar(dvh);
                }

            }
        }

        //public List<Services.DVH> ValidarDigitoVerificador()
        //{
        //    List<Services.DVH> registrosInvalidos = new List<Services.DVH>();

        //    // Listar todas las entidades de la base de datos
        //    List<BE.Pelicula> peliculas = bllPelicula.Listar();
        //    List<BE.Emocion> emociones = bllEmocion.Listar();
        //    List<BE.Libro> libros = bllLibro.Listar();
        //    List<BE.Usuario> usuarios = bllUsuario.Listar();



        //    List<Services.DVH> dVHs = Listar();


        //    foreach (var pelicula in peliculas)
        //    {
        //        var cadena = bllPelicula.Concatenar(pelicula);
        //        var hash = ObtenerDV(cadena);

        //        var dvh = dVHs.FirstOrDefault(d => d.Tabla == "PELICULA" && d.Registro == pelicula.ID);
        //        if (dvh != null && hash != dvh.DV)
        //        {
        //            registrosInvalidos.Add(dvh);
        //        }


        //    }

        //    foreach (var emocion in emociones)
        //    {
        //        var cadena = bllEmocion.Concatenar(emocion);
        //        var hash = ObtenerDV(cadena);
        //        var dvh = dVHs.FirstOrDefault(d => d.Tabla == "EMOCION" && d.Registro == emocion.ID);
        //        if (dvh != null && hash != dvh.DV)
        //        {
        //            registrosInvalidos.Add(dvh);
        //        }
        //    }


        //    foreach (var usuario in usuarios)
        //    {
        //        var cadena = bllUsuario.Concatenar(usuario);
        //        var hash = ObtenerDV(cadena);

        //        var dvh = dVHs.FirstOrDefault(d => d.Tabla == "USUARIO" && d.Registro == usuario.ID);
        //        if (dvh != null && hash != dvh.DV)
        //        {
        //            registrosInvalidos.Add(dvh);
        //        }
        //    }


        //    foreach (var libro in libros)
        //    {
        //        var cadena = bllLibro.Concatenar(libro);
        //        var hash = ObtenerDV(cadena);

        //        var dvh = dVHs.FirstOrDefault(d => d.Tabla == "LIBRO" && d.Registro == libro.ID);
        //        if (dvh != null && hash != dvh.DV)
        //        {
        //            registrosInvalidos.Add(dvh);
        //        }
        //    }

        //    return registrosInvalidos;
        //}

        public List<RegistroInvalido> ValidarDigitoVerificador()
        {
            List<RegistroInvalido> registrosInvalidos = new List<RegistroInvalido>();

            List<BE.Pelicula> peliculas = bllPelicula.Listar();
            List<BE.Emocion> emociones = bllEmocion.Listar();
            List<BE.Libro> libros = bllLibro.Listar();
            List<BE.Usuario> usuarios = bllUsuario.Listar();
            List<Services.Bitacora> bitacoras = bllBitacora.Listar();

            List<Services.DVH> dVHs = Listar();

            foreach (var dvh in dVHs)
            {
                bool registroValido = true;
                string estado = "";

                switch (dvh.Tabla)
                {
                    case "PELICULA":
                        var pelicula = peliculas.FirstOrDefault(p => p.ID == dvh.Registro);
                        if (pelicula != null)
                        {
                            var cadena = bllPelicula.Concatenar(pelicula);
                            var hash = ObtenerDV(cadena);
                            if (hash != dvh.DV)
                            {
                                registroValido = false;
                                estado = "Modificado";
                            }
                        }
                        else
                        {
                            registroValido = false;
                            estado = "Eliminado";
                        }
                        break;

                    case "EMOCION":
                        var emocion = emociones.FirstOrDefault(e => e.ID == dvh.Registro);
                        if (emocion != null)
                        {
                            var cadena = bllEmocion.Concatenar(emocion);
                            var hash = ObtenerDV(cadena);
                            if (hash != dvh.DV)
                            {
                                registroValido = false;
                                estado = "Modificado";
                            }
                        }
                        else
                        {
                            registroValido = false;
                            estado = "Eliminado";
                        }
                        break;

                    case "LIBRO":
                        var libro = libros.FirstOrDefault(l => l.ID == dvh.Registro);
                        if (libro != null)
                        {
                            var cadena = bllLibro.Concatenar(libro);
                            var hash = ObtenerDV(cadena);
                            if (hash != dvh.DV)
                            {
                                registroValido = false;
                                estado = "Modificado";
                            }
                        }
                        else
                        {
                            registroValido = false;
                            estado = "Eliminado";
                        }
                        break;

                    case "USUARIO":
                        var usuario = usuarios.FirstOrDefault(u => u.ID == dvh.Registro);
                        if (usuario != null)
                        {
                            var cadena = bllUsuario.Concatenar(usuario);
                            var hash = ObtenerDV(cadena);
                            if (hash != dvh.DV)
                            {
                                registroValido = false;
                                estado = "Modificado";
                            }
                        }
                        else
                        {
                            registroValido = false;
                            estado = "Eliminado";
                        }
                        break;

                    case "BITACORA":
                        var bitacora = bitacoras.FirstOrDefault(b => b.ID == dvh.Registro);
                        if (bitacora != null)
                        {
                            var cadena = bllBitacora.Concatenar(bitacora);
                            var hash = ObtenerDV(cadena);
                            if (hash != dvh.DV)
                            {
                                registroValido = false;
                                estado = "Modificado";
                            }
                        }
                        else
                        {
                            registroValido = false;
                            estado = "Eliminado";
                        }
                        break;

                    default:
                        registroValido = false;
                        estado = "Tabla desconocida";
                        break;
                }

                if (!registroValido)
                {
                    registrosInvalidos.Add(new RegistroInvalido { DVH = dvh, Estado = estado });
                }
            }

            return registrosInvalidos;
        }


        public bool ValidarCantidadRegistros<T>(List<T> entidades, List<Services.DVH> dVHs, string tabla)
        {
            bool ok = true;

            
            List<Services.DVH> dVHsFiltrados = dVHs.Where(dvh => dvh.Tabla == tabla).ToList();

            
            if (entidades.Count != dVHsFiltrados.Count)
            {
                ok = false;
            }

            return ok;
        }

        public void EliminarRegistro(string tabla, int id)
        {
            mpDvh.Delete(tabla, id);
        }

        public void LimpiarRegistrosOrfandades<T>(List<T> entidades, string tabla) where T : class
        {
            // 1) Obtener todos los DVH de esa tabla
            List<Services.DVH> dvhsTabla = Listar().Where(x => x.Tabla == tabla).ToList();

            // 2) Extraer los IDs reales existentes
            List<int> idsReales = new List<int>();

            // Detectar tipo de entidad y obtener ID
            foreach (var ent in entidades)
            {
                int id = 0;

                if (ent is BE.Pelicula p) id = p.ID;
                else if (ent is BE.Libro l) id = l.ID;
                else if (ent is BE.Emocion e) id = e.ID;
                else if (ent is BE.Usuario u) id = u.ID;
                else if (ent is Services.Bitacora b) id = b.ID;

                idsReales.Add(id);
            }

            // 3) Buscar DVH huérfanos y eliminarlos
            foreach (var dvh in dvhsTabla)
            {
                if (!idsReales.Contains(dvh.Registro))
                {
                    // Este DVH está huérfano ➜ lo eliminamos
                    EliminarRegistro(tabla, dvh.Registro);
                }
            }
        }


    }
}
