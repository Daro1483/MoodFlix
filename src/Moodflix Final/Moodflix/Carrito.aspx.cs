using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BE;
using BLL;
using Services;

namespace Moodflix
{
    public partial class Carrito : System.Web.UI.Page, IIdiomaObserver
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Se pone aca, en los casos que estoy en la misma pagina y quiero cambiar idioma
            GestorIdioma.Instancia.Suscribir(this);
            MostrarCarrito();
            if (!IsPostBack)
            {
                AplicarTraducciones();                
                
            }            
        }
        public void ActualizarIdioma(Idioma idioma)
        {
            AplicarTraducciones();
            MostrarCarrito();
            
        }

        private void AplicarTraducciones()
        {
            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
            if (idioma == null) return;

            // Título principal
            lblTitulo.InnerText = idioma.Traducciones["Carrito_lblTitulo"];

        }
        public void MostrarCarrito()
        {

            List<Producto> carrito = Session["Carrito"] as List<Producto>;
            if (carrito == null || carrito.Count == 0)
            {
                var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
                string mensaje = idioma.Traducciones["Carrito_msgVacio"];
                MostrarError(mensaje);
                return;
            }


            carritoContainer.Controls.Clear();
            


            foreach (var item in carrito)
            {
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", "col-12 col-md-6 col-lg-4 text-center");

                ImageButton btn = new ImageButton();
                btn.ImageUrl = item.Uri;
                btn.Width = Unit.Pixel(210);

                HtmlGenericControl h3 = new HtmlGenericControl("h3");
                h3.Attributes.Add("class", "w-100");
                h3.InnerText = item.Nombre;

                HtmlGenericControl divPrecio = new HtmlGenericControl("div");
                divPrecio.Attributes.Add("class", "card-text");
                divPrecio.InnerText = item.Precio.ToString("C");

                div.Controls.Add(btn);
                div.Controls.Add(h3);
                div.Controls.Add(divPrecio);

                carritoContainer.Controls.Add(div);
            }
        }

        private void MostrarError(string mensaje)
        {
            pnlError.Visible = true;
            lblError.Text = mensaje;
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }

    }
}