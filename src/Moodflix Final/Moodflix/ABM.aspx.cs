using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using Services;

namespace Moodflix
{
    public partial class ABM : System.Web.UI.Page, IIdiomaObserver
    {
        BLL.Pelicula bllPelicula = new BLL.Pelicula();
        BLL.Libro bllLibro = new BLL.Libro();
        BLL.Emocion bllEmocion = new BLL.Emocion();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();
        BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Observer
            GestorIdioma.Instancia.Suscribir(this);

            // 2) Tomamos usuario de sesión
            var usuario = Session["Usuario"] as BE.Usuario;

            // 3) Validación de permisos (NO validamos sesión; eso lo hace Site.Master)
            if (usuario != null && !bllUsuario.UsuarioTienePatente(usuario, "ACCESO_ABM"))
            {
                Response.Redirect("~/SinPermisos.aspx");
                return;
            }

            // 4) Primera carga
            if (!IsPostBack)
            {
                AplicarTraducciones();
                CargarEmociones();
                CargarPeliculas();
                CargarLibros();
            }
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            AplicarTraducciones();
            CargarPeliculas();
            CargarLibros();
        }

        private void AplicarTraducciones()
        {
            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
            if (idioma == null) return;

            lblTituloPelicula.InnerText = idioma.Traducciones["ABM_lblTituloPeliculas"];
            lblNombrePelicula.Text = idioma.Traducciones["ABM_lblNombrePeliculas"];
            lblDescripcionPelicula.Text = idioma.Traducciones["ABM_lblDescripcionPeliculas"];
            lblFechaPelicula.Text = idioma.Traducciones["ABM_lblFechaPeliculas"];
            lblPrecioPelicula.Text = idioma.Traducciones["ABM_lblPrecioPeliculas"];
            lblEmocionPelicula.Text = idioma.Traducciones["ABM_lblEmocionPeliculas"];
            lblGeneroPelicula.Text = idioma.Traducciones["ABM_lblGeneroPeliculas"];
            lblDirectorPelicula.Text = idioma.Traducciones["ABM_lblDirectorPeliculas"];

            btnAgregarPelicula.Text = idioma.Traducciones["ABM_btnAgregarPelicula"];
            btnActualizarPelicula.Text = idioma.Traducciones["ABM_btnActualizarPelicula"];
            btnEliminarPelicula.Text = idioma.Traducciones["ABM_btnEliminarPelicula"];

            gvPeliculas.Columns[0].HeaderText = idioma.Traducciones["ABM_grd_PeliculaID"];
            gvPeliculas.Columns[1].HeaderText = idioma.Traducciones["ABM_grd_PeliculaNombre"];
            gvPeliculas.Columns[2].HeaderText = idioma.Traducciones["ABM_grd_PeliculaDescripcion"];
            gvPeliculas.Columns[3].HeaderText = idioma.Traducciones["ABM_grd_PeliculaFecha"];
            gvPeliculas.Columns[4].HeaderText = idioma.Traducciones["ABM_grd_PeliculaPrecio"];
            gvPeliculas.Columns[5].HeaderText = idioma.Traducciones["ABM_grd_PeliculaURI"];
            gvPeliculas.Columns[6].HeaderText = idioma.Traducciones["ABM_grd_PeliculaEmocion"];
            gvPeliculas.Columns[7].HeaderText = idioma.Traducciones["ABM_grd_PeliculaGenero"];
            gvPeliculas.Columns[8].HeaderText = idioma.Traducciones["ABM_grd_PeliculaDirector"];

            lblTituloLibro.InnerText = idioma.Traducciones["ABM_lblTituloLibros"];
            lblNombreLibro.Text = idioma.Traducciones["ABM_lblNombreLibros"];
            lblDescripcionLibro.Text = idioma.Traducciones["ABM_lblDescripcionLibros"];
            lblFechaLibro.Text = idioma.Traducciones["ABM_lblFechaLibros"];
            lblPrecioLibro.Text = idioma.Traducciones["ABM_lblPrecioLibros"];
            lblEmocionLibro.Text = idioma.Traducciones["ABM_lblEmocionLibros"];
            lblAutorLibro.Text = idioma.Traducciones["ABM_lblAutorLibros"];
            lblEditorialLibro.Text = idioma.Traducciones["ABM_lblEditorialLibros"];

            btnAgregarLibro.Text = idioma.Traducciones["ABM_btnAgregarLibro"];
            btnActualizarLibro.Text = idioma.Traducciones["ABM_btnActualizarLibro"];
            btnEliminarLibro.Text = idioma.Traducciones["ABM_btnEliminarLibro"];

            gvLibros.Columns[0].HeaderText = idioma.Traducciones["ABM_grd_LibroID"];
            gvLibros.Columns[1].HeaderText = idioma.Traducciones["ABM_grd_LibroNombre"];
            gvLibros.Columns[2].HeaderText = idioma.Traducciones["ABM_grd_LibroDescripcion"];
            gvLibros.Columns[3].HeaderText = idioma.Traducciones["ABM_grd_LibroFecha"];
            gvLibros.Columns[4].HeaderText = idioma.Traducciones["ABM_grd_LibroPrecio"];
            gvLibros.Columns[5].HeaderText = idioma.Traducciones["ABM_grd_LibroURI"];
            gvLibros.Columns[6].HeaderText = idioma.Traducciones["ABM_grd_LibroEmocion"];
            gvLibros.Columns[7].HeaderText = idioma.Traducciones["ABM_grd_LibroAutor"];
            gvLibros.Columns[8].HeaderText = idioma.Traducciones["ABM_grd_LibroEditorial"];

            rfvNombrePelicula.ErrorMessage = idioma.Traducciones["ABM_ErrorNombreObligatorio"];
            rfvDescripcionPelicula.ErrorMessage = idioma.Traducciones["ABM_ErrorDescripcionObligatoria"];
            rfvFechaPelicula.ErrorMessage = idioma.Traducciones["ABM_ErrorFechaObligatoria"];
            rfvPrecioPelicula.ErrorMessage = idioma.Traducciones["ABM_ErrorPrecioObligatorio"];
            rfvUriPelicula.ErrorMessage = idioma.Traducciones["ABM_ErrorUriObligatoria"];
            rfvEmocionPelicula.ErrorMessage = idioma.Traducciones["ABM_ErrorEmocionObligatoria"];
            rfvGenero.ErrorMessage = idioma.Traducciones["ABM_ErrorGeneroObligatorio"];
            rfvDirector.ErrorMessage = idioma.Traducciones["ABM_ErrorDirectorObligatorio"];

            rfvNombreLibro.ErrorMessage = idioma.Traducciones["ABM_ErrorNombreObligatorio"];
            rfvDescripcionLibro.ErrorMessage = idioma.Traducciones["ABM_ErrorDescripcionObligatoria"];
            rfvFechaLibro.ErrorMessage = idioma.Traducciones["ABM_ErrorFechaObligatoria"];
            rfvPrecioLibro.ErrorMessage = idioma.Traducciones["ABM_ErrorPrecioObligatorio"];
            rfvUriLibro.ErrorMessage = idioma.Traducciones["ABM_ErrorUriObligatoria"];
            rfvEmocionLibro.ErrorMessage = idioma.Traducciones["ABM_ErrorEmocionObligatoria"];
            rfvAutor.ErrorMessage = idioma.Traducciones["ABM_ErrorAutorObligatorio"];
            rfvEditorial.ErrorMessage = idioma.Traducciones["ABM_ErrorEditorialObligatoria"];
        }


        protected void btnAgregarPelicula_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Pelicula pelicula = new Pelicula
                {
                    Nombre = txtNombrePelicula.Text,
                    Descripcion = txtDescripcionPelicula.Text,
                    Fecha = DateTime.Parse(txtFechaPelicula.Value),
                    Precio = float.Parse(txtPrecioPelicula.Text),
                    Uri = txtUriPelicula.Text,
                    Emocion = new Emocion { ID = int.Parse(ddlEmocionPelicula.SelectedValue) },
                    Genero = txtGenero.Text,
                    Director = txtDirector.Text
                };

                bllPelicula.Insertar(pelicula);
                CargarPeliculas();

                bllDvh.Recalcular(bllDvh.Listar(), bllPelicula.Listar());
                bllDvv.Recalcular();

                ClientScript.RegisterStartupScript(this.GetType(), "alert4", "alert('Producto cargado con éxito.');", true);
                
                //Bitácora
                var usuario = Session["Usuario"] as BE.Usuario;

                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.ABM,
                        Operacion = TipoOperacion.AltaPelicula,
                    };

                    new BLL.Bitacora().Insertar(bitacora);


                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }

                    
            }
        }

        protected void btnActualizarPelicula_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int id = int.Parse(gvPeliculas.SelectedRow.Cells[0].Text);
                Pelicula pelicula = new Pelicula
                {
                    ID = id,
                    Nombre = txtNombrePelicula.Text,
                    Descripcion = txtDescripcionPelicula.Text,
                    Fecha = DateTime.Parse(txtFechaPelicula.Value),
                    Precio = float.Parse(txtPrecioPelicula.Text),
                    Uri = txtUriPelicula.Text,
                    Emocion = new Emocion { ID = int.Parse(ddlEmocionPelicula.SelectedValue) },
                    Genero = txtGenero.Text,
                    Director = txtDirector.Text
                };

                bllPelicula.Actualizar(pelicula);
                CargarPeliculas();


                bllDvh.Recalcular(bllDvh.Listar(), bllPelicula.Listar());
                bllDvv.Recalcular();

                ClientScript.RegisterStartupScript(this.GetType(), "alert5", "alert('Producto actualizado con éxito.');", true);

                //Bitácora
                var usuario = Session["Usuario"] as BE.Usuario;

                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.ABM,
                        Operacion = TipoOperacion.ModificarPelicula,
                    };

                    new BLL.Bitacora().Insertar(bitacora);


                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }
            }
        }

        protected void btnEliminarPelicula_OnClick(object sender, EventArgs e)
        {
            int id = int.Parse(gvPeliculas.SelectedRow.Cells[0].Text);
            bllPelicula.Eliminar(id);
            CargarPeliculas();

            //Elimina el regsitro de DVH
            bllDvh.EliminarRegistro("PELICULA", id);


            bllDvh.Recalcular(bllDvh.Listar(), bllPelicula.Listar());
            bllDvv.Recalcular();

            ClientScript.RegisterStartupScript(this.GetType(), "alert6", "alert('Producto eliminado con éxito.');", true);

            //Bitácora
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.ABM,
                    Operacion = TipoOperacion.BorrarPelicula,
                };

                new BLL.Bitacora().Insertar(bitacora);


                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }
        }

        protected void gvPeliculas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(gvPeliculas.SelectedRow.Cells[0].Text);
            Pelicula pelicula = bllPelicula.ObtenerPorId(id);
            txtNombrePelicula.Text = pelicula.Nombre;
            txtDescripcionPelicula.Text = pelicula.Descripcion;
            txtFechaPelicula.Value = pelicula.Fecha.ToString("yyyy-MM-dd");
            txtPrecioPelicula.Text = pelicula.Precio.ToString();
            txtUriPelicula.Text = pelicula.Uri;
            ddlEmocionPelicula.SelectedValue = pelicula.Emocion.ID.ToString();
            txtGenero.Text = pelicula.Genero;
            txtDirector.Text = pelicula.Director;
        }

        protected void btnAgregarLibro_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Libro libro = new Libro
                {
                    Nombre = txtNombreLibro.Text,
                    Descripcion = txtDescripcionLibro.Text,
                    Fecha = DateTime.Parse(txtFechaLibro.Text),
                    Precio = float.Parse(txtPrecioLibro.Text),
                    Uri = txtUriLibro.Text,
                    Emocion = new Emocion { ID = int.Parse(ddlEmocionLibro.SelectedValue) },
                    Autor = txtAutor.Text,
                    Editorial = txtEditorial.Text
                };

                bllLibro.Insertar(libro);
                CargarLibros();


                bllDvh.Recalcular(bllDvh.Listar(), bllLibro.Listar());
                bllDvv.Recalcular();
                ClientScript.RegisterStartupScript(this.GetType(), "alert1", "alert('Producto cargado con éxito.');", true);

                //Bitácora
                var usuario = Session["Usuario"] as BE.Usuario;

                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.ABM,
                        Operacion = TipoOperacion.AltaLibro,
                    };

                    new BLL.Bitacora().Insertar(bitacora);


                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }
            }
        }

        protected void btnActualizarLibro_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int id = int.Parse(gvLibros.SelectedRow.Cells[0].Text);
                Libro libro = new Libro
                {
                    ID = id,
                    Nombre = txtNombreLibro.Text,
                    Descripcion = txtDescripcionLibro.Text,
                    Fecha = DateTime.Parse(txtFechaLibro.Text),
                    Precio = float.Parse(txtPrecioLibro.Text),
                    Uri = txtUriLibro.Text,
                    Emocion = new Emocion { ID = int.Parse(ddlEmocionLibro.SelectedValue) },
                    Autor = txtAutor.Text,
                    Editorial = txtEditorial.Text
                };

                bllLibro.Actualizar(libro);
                CargarLibros();

                bllDvh.Recalcular(bllDvh.Listar(), bllLibro.Listar());
                bllDvv.Recalcular();
                ClientScript.RegisterStartupScript(this.GetType(), "alert2", "alert('Producto actualizado con éxito.');", true);
                //Bitácora
                var usuario = Session["Usuario"] as BE.Usuario;

                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.ABM,
                        Operacion = TipoOperacion.ModificarLibro,
                    };

                    new BLL.Bitacora().Insertar(bitacora);


                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }
            }
        }

        protected void btnEliminarLibro_OnClick(object sender, EventArgs e)
        {
            int id = int.Parse(gvLibros.SelectedRow.Cells[0].Text);
            bllLibro.Eliminar(id);
            CargarLibros();

            // ELIMINAR DVH ASOCIADO 
            bllDvh.EliminarRegistro("LIBRO", id);


            bllDvh.Recalcular(bllDvh.Listar(), bllLibro.Listar());
            bllDvv.Recalcular();

            ClientScript.RegisterStartupScript(this.GetType(), "alert3", "alert('Producto eliminado con éxito.');", true);

            //Bitácora
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.ABM,
                    Operacion = TipoOperacion.BorrarLibro,
                };

                new BLL.Bitacora().Insertar(bitacora);


                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }
        }

        protected void gvLibros_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int id = int.Parse(gvLibros.SelectedRow.Cells[0].Text);
            Libro libro = bllLibro.ObtenerPorId(id);
            txtNombreLibro.Text = libro.Nombre;
            txtDescripcionLibro.Text = libro.Descripcion;
            txtFechaLibro.Text = libro.Fecha.ToString("yyyy-MM-dd");
            txtPrecioLibro.Text = libro.Precio.ToString();
            txtUriLibro.Text = libro.Uri;
            ddlEmocionLibro.SelectedValue = libro.Emocion.ID.ToString();
            txtAutor.Text = libro.Autor;
            txtEditorial.Text = libro.Editorial;
        }


        private void CargarEmociones()
        {
            ddlEmocionPelicula.DataSource = bllEmocion.Listar();
            ddlEmocionPelicula.DataTextField = "Nombre";
            ddlEmocionPelicula.DataValueField = "ID";
            ddlEmocionPelicula.DataBind();

            ddlEmocionLibro.DataSource = bllEmocion.Listar();
            ddlEmocionLibro.DataTextField = "Nombre";
            ddlEmocionLibro.DataValueField = "ID";
            ddlEmocionLibro.DataBind();
        }

        private void CargarPeliculas()
        {
            gvPeliculas.DataSource = bllPelicula.Listar();
            gvPeliculas.DataBind();
        }

        private void CargarLibros()
        {
            gvLibros.DataSource = bllLibro.Listar();
            gvLibros.DataBind();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
