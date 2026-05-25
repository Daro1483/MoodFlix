using System;
using Services;
using BE;

namespace Moodflix
{
    public partial class SinPermisos : System.Web.UI.Page, IIdiomaObserver
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Suscribirse SIEMPRE al gestor de idioma
            GestorIdioma.Instancia.Suscribir(this);

            // 2) Cargar traducciones SOLO en la primera carga
            if (!IsPostBack)
            {
                AplicarTraducciones();
            }
        }

        // ============================================================
        //   OBSERVER
        // ============================================================
        public void ActualizarIdioma(Idioma idioma)
        {
            AplicarTraducciones();
        }

        // ============================================================
        //   TRADUCCIÓN
        // ============================================================
        private void AplicarTraducciones()
        {
            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
            if (idioma == null || idioma.Traducciones == null) return;

            try
            {
                lblTitulo.InnerText = idioma.Traducciones["SinPermisos_Titulo"];
                lblMensaje1.InnerText = idioma.Traducciones["SinPermisos_Mensaje1"];
                lblMensaje2.InnerText = idioma.Traducciones["SinPermisos_Mensaje2"];
                btnVolver.Text = idioma.Traducciones["SinPermisos_BtnVolver"];
            }
            catch
            {
                // Evita caídas si falta una clave
            }
        }

        // ============================================================
        //   BOTÓN VOLVER
        // ============================================================
        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Emociones.aspx");
        }

        // ============================================================
        //   DESUSCRIBIR OBSERVER
        // ============================================================
        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
