using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Services;

namespace BLL
{
    public class DVV
    {
        private MP_Dvv mpDvv = new MP_Dvv();

        private Usuario bllUsuario = new Usuario();
        private Emocion bllEmocion = new Emocion();
        private Pelicula bllPelicula = new Pelicula();
        private Libro bllLibro = new Libro();
        private Bitacora bllBitacora = new Bitacora();

        // =============================================================
        // CRUD DVV
        // =============================================================
        public List<Services.DVV> Listar()
        {
            return mpDvv.GetAll();
        }

        public int Insertar(Services.DVV dvv)
        {
            return mpDvv.Insert(dvv);
        }

        public int Actualizar(Services.DVV dvv)
        {
            return mpDvv.Update(dvv);
        }

        public void BorrarRegistros()
        {
            mpDvv.DeleteAll();
        }

        // =============================================================
        // HASH
        // =============================================================
        public string ObtenerDV(string cadena)
        {
            return CryptoManager.Hash(cadena);
        }

        // =============================================================
        // CONCATENACIÓN DETERMINÍSTICA
        // =============================================================
        private string ConcatenarColumna<T>(List<T> entities, string columnName)
        {
            var ordenadas = entities
                .OrderBy(e => ObtenerValorColumna(e, "ID"))
                .ToList();

            return string.Join("|", ordenadas.Select(e => ObtenerValorColumna(e, columnName)));
        }

        // =============================================================
        // RECÁLCULO DVV
        // =============================================================
        public void Recalcular()
        {
            List<Services.DVV> dVVs = Listar();

            var entitiesAndColumns = new List<(List<object> entities, List<string> columns)>
            {
                (bllLibro.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "NOMBRE", "DESCRIPCION", "FECHA", "AUTOR", "EDITORIAL", "ID_EMOCION", "URI_RELATIVO", "PRECIO" }),

                (bllEmocion.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "NOMBRE", "URI_RELATIVO" }),

                (bllPelicula.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "NOMBRE", "DESCRIPCION", "FECHA", "GENERO", "DIRECTOR", "ID_EMOCION", "URI_RELATIVO", "PRECIO" }),

                (bllUsuario.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "USERNAME", "EMAIL", "PASSWORD" }),

                (bllBitacora.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "FECHA", "ID_USUARIO", "MODULO", "OPERACION" }
)
            };

            foreach (var (entities, columns) in entitiesAndColumns)
            {
                if (!entities.Any())
                    continue;

                string tabla = entities.First().GetType().Name.ToUpper();

                for (int i = 0; i < columns.Count; i++)
                {
                    string hash = ObtenerDV(ConcatenarColumna(entities, columns[i]));
                    int nroColumna = i + 1;

                    var dvv = dVVs.FirstOrDefault(d => d.Tabla == tabla && d.Columna == nroColumna);

                    if (dvv != null)
                    {
                        dvv.DV = hash;
                        Actualizar(dvv);
                    }
                    else
                    {
                        Insertar(new Services.DVV
                        {
                            Tabla = tabla,
                            Columna = nroColumna,
                            DV = hash
                        });
                    }
                }
            }
        }

        // =============================================================
        // VALIDACIÓN DVV
        // =============================================================
        public List<ColumnaInvalida> ValidarDigitoVerificador()
        {
            List<Services.DVV> dVVs = Listar();
            List<ColumnaInvalida> columnasInvalidas = new List<ColumnaInvalida>();

            var entitiesAndColumns = new List<(List<object> entities, List<string> columns)>
            {
                (bllLibro.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "NOMBRE", "DESCRIPCION", "FECHA", "AUTOR", "EDITORIAL", "ID_EMOCION", "URI_RELATIVO", "PRECIO" }),

                (bllEmocion.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "NOMBRE", "URI_RELATIVO" }),

                (bllPelicula.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "NOMBRE", "DESCRIPCION", "FECHA", "GENERO", "DIRECTOR", "ID_EMOCION", "URI_RELATIVO", "PRECIO" }),

                (bllUsuario.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "USERNAME", "EMAIL", "PASSWORD" }),

                // MISMO ORDEN QUE EN RECALCULAR
                (bllBitacora.Listar().Cast<object>().ToList(),
                    new List<string> { "ID", "FECHA", "ID_USUARIO", "MODULO", "OPERACION" }
)
            };

            foreach (var (entities, columns) in entitiesAndColumns)
            {
                if (!entities.Any())
                    continue;

                string tabla = entities.First().GetType().Name.ToUpper();

                for (int i = 0; i < columns.Count; i++)
                {
                    string hash = ObtenerDV(ConcatenarColumna(entities, columns[i]));
                    int nroColumna = i + 1;

                    var dvv = dVVs.FirstOrDefault(d => d.Tabla == tabla && d.Columna == nroColumna);

                    if (dvv != null && dvv.DV != hash)
                    {
                        columnasInvalidas.Add(new ColumnaInvalida
                        {
                            DVV = dvv,
                            Estado = "Modificada"
                        });
                    }
                    else if (dvv == null)
                    {
                        columnasInvalidas.Add(new ColumnaInvalida
                        {
                            DVV = new Services.DVV
                            {
                                Tabla = tabla,
                                Columna = nroColumna,
                                DV = hash
                            },
                            Estado = "Eliminada"
                        });
                    }
                }
            }

            return columnasInvalidas;
        }

        // =============================================================
        // NORMALIZACIÓN DE VALORES
        // =============================================================
        private string ObtenerValorColumna<T>(T entity, string columnName)
        {
            if (entity is BE.Libro libro)
            {
                switch (columnName)
                {
                    case "ID": return libro.ID.ToString();
                    case "NOMBRE": return libro.Nombre ?? "";
                    case "DESCRIPCION": return libro.Descripcion ?? "";
                    case "FECHA": return libro.Fecha.ToString("yyyyMMdd");
                    case "AUTOR": return libro.Autor ?? "";
                    case "EDITORIAL": return libro.Editorial ?? "";
                    case "ID_EMOCION": return libro.Emocion.ID.ToString();
                    case "URI_RELATIVO": return libro.Uri ?? "";
                    case "PRECIO": return libro.Precio.ToString();
                }
            }

            if (entity is BE.Emocion emocion)
            {
                switch (columnName)
                {
                    case "ID": return emocion.ID.ToString();
                    case "NOMBRE": return emocion.TipoEmocion.ToString();
                    case "URI_RELATIVO": return emocion.Uri ?? "";
                }
            }

            if (entity is BE.Pelicula pelicula)
            {
                switch (columnName)
                {
                    case "ID": return pelicula.ID.ToString();
                    case "NOMBRE": return pelicula.Nombre ?? "";
                    case "DESCRIPCION": return pelicula.Descripcion ?? "";
                    case "FECHA": return pelicula.Fecha.ToString("yyyyMMdd");
                    case "GENERO": return pelicula.Genero ?? "";
                    case "DIRECTOR": return pelicula.Director ?? "";
                    case "ID_EMOCION": return pelicula.Emocion.ID.ToString();
                    case "URI_RELATIVO": return pelicula.Uri ?? "";
                    case "PRECIO": return pelicula.Precio.ToString();
                }
            }

            if (entity is BE.Usuario usuario)
            {
                switch (columnName)
                {
                    case "ID": return usuario.ID.ToString();
                    case "USERNAME": return usuario.Username ?? "";
                    case "EMAIL": return usuario.Email ?? "";
                    case "PASSWORD": return usuario.Password ?? "";
                }
            }

            if (entity is Services.Bitacora bitacora)
            {
                switch (columnName)
                {
                    case "ID": return bitacora.ID.ToString();
                    case "ID_USUARIO": return bitacora.User.ID.ToString();
                    case "FECHA": return bitacora.Fecha.ToString("yyyyMMddHHmmss");
                    case "OPERACION": return bitacora.Operacion.ToString();
                    case "MODULO": return bitacora.Modulo.ToString();
                }
            }

            return "";
        }
    }
}
