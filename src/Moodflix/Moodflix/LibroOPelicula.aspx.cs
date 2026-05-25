using Services;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Moodflix
{
    public partial class LibroOPelicula : System.Web.UI.Page, IIdiomaObserver
    {
        BLL.DVV bllDvv = new BLL.DVV();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();

        protected void Page_Load(object sender, EventArgs e)
        {
            // El observer SIEMPRE se registra antes de cualquier acción
            GestorIdioma.Instancia.Suscribir(this);

            if (!IsPostBack)
            {
                // Idioma actual
                var idiomaActual = GestorIdioma.Instancia.ObtenerIdiomaActual();
                ddlIdiomas.SelectedValue = idiomaActual?.Codigo ?? "ES";

                ActualizarIdioma(idiomaActual);
                SetNavbar();
            }
            else
            {
                // Si hay postback y el usuario está logueado, mantener la navbar actualizada
                SetNavbar();
            }
        }

        // ==========================================================
        // NAVBAR (Roles / Patentes / Visibilidad)
        // ==========================================================
        private void SetNavbar()
        {
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario == null)
            {
                PlantillaUserAnonimo.Visible = true;
                PlantillaUserRegistrado.Visible = false;
                PlantillaHerramientas.Visible = false;
                return;
            }

            PlantillaUserAnonimo.Visible = false;
            PlantillaUserRegistrado.Visible = true;
            PlantillaIdioma.Visible = true;

            LinkProfile.Text = usuario.Username;

            // Permisos EXACTOS
            bool accBitacora = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_BITACORA");
            bool accSeguridad = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_SEGURIDAD");
            bool accPerfiles = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_PERFILES");
            bool accABM = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_ABM");
            bool accUsuariosBloqueados = accSeguridad;

            bool mostrar = accBitacora || accSeguridad || accPerfiles || accABM || accUsuariosBloqueados;
            PlantillaHerramientas.Visible = mostrar;

            // 🔥 SOLO construir el DDL en primera carga
            if (!IsPostBack)
            {
                ddlHerramientas.Items.Clear();
                ddlHerramientas.Items.Add(new ListItem("Herramientas", ""));

                if (accBitacora)
                    ddlHerramientas.Items.Add(new ListItem("Bitácora", "Bitacora"));

                if (accSeguridad)
                    ddlHerramientas.Items.Add(new ListItem("Seguridad", "Seguridad"));

                if (accPerfiles)
                    ddlHerramientas.Items.Add(new ListItem("Gestión de Perfiles", "Perfiles"));

                if (accABM)
                    ddlHerramientas.Items.Add(new ListItem("ABM", "ABM"));

                if (accUsuariosBloqueados)
                    ddlHerramientas.Items.Add(new ListItem("Usuarios bloqueados", "UsuariosBloqueados"));
            }
        }



        // ==========================================================
        // EVENTOS DE NAVEGACIÓN
        // ==========================================================

        protected void imgbVerPeliculas_OnClick(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Peliculas.aspx");
        }

        protected void imgbVerLibros_OnClick(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Libros.aspx");
        }

        protected void imgbVerTodo_OnClick(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("LibrosYPeliculas.aspx");
        }

        protected void linkInitLogin_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void LinkInitRegistro_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Registrarse.aspx");
        }

        protected void LinkLogout_OnClick(object sender, EventArgs e)
        {
            // Registrar bitácora
            try
            {
                var usuario = bllUsuario.GetUserByUsername(User.Identity.Name);

                var bitacora = new Services.Bitacora
                {
                    User = usuario,
                    Fecha = DateTime.Now,
                    Operacion = TipoOperacion.Logout,
                    Modulo = TipoModulo.LibroOPelicula
                };

                bllBitacora.Insertar(bitacora);
                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }
            catch { }

            Session.Clear();
            FormsAuthentication.SignOut();

            Response.Redirect("Login.aspx");
        }

        // ==========================================================
        // HERRAMIENTAS (DDL)
        // ==========================================================

        protected void ddlActions_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string v = ddlHerramientas.SelectedValue;

            switch (v)
            {
                case "Bitacora":
                    Response.Redirect("Bitacora.aspx");
                    break;

                case "Seguridad":
                    Response.Redirect("Backup.aspx");
                    break;

                case "Perfiles":
                    Response.Redirect("Perfiles.aspx");
                    break;

                case "ABM":
                    Response.Redirect("ABM.aspx");
                    break;
                case "UsuariosBloqueados":
                    Response.Redirect("UsuariosBloqueados.aspx");
                    break;

            }
        }

        // ==========================================================
        // IDIOMA
        // ==========================================================

        public void ActualizarIdioma(BE.Idioma idioma)
        {
            if (idioma == null || idioma.Traducciones == null)
                return;

            try
            {
                // Links principales
                linkInitLogin.Text = idioma.Traducciones["Emociones_LinkInicioSesion"];
                LinkInitRegistro.Text = idioma.Traducciones["Emociones_LinkRegistro"];
                LinkLogout.Text = idioma.Traducciones["Emociones_LinkCierreSesion"];

                // Traducción dinámica del DDL
                foreach (ListItem item in ddlHerramientas.Items)
                {
                    switch (item.Value)
                    {
                        case "":
                            item.Text = idioma.Traducciones["Emociones_Herramientas"];
                            break;
                        case "Bitacora":
                            item.Text = idioma.Traducciones["Emociones_Bitacora"];
                            break;
                        case "Seguridad":
                            item.Text = idioma.Traducciones["SiteMaster_LinkSeguridad"];
                            break;
                        case "Perfiles":
                            item.Text = idioma.Traducciones["SiteMaster_LinkPerfiles"];
                            break;
                        case "ABM":
                            item.Text = idioma.Traducciones["Emociones_ABM"];
                            break;
                        case "UsuariosBloqueados":
                            item.Text = idioma.Traducciones["SiteMaster_LinkUsuariosBloqueados"];
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ActualizarIdioma (LibroOPelicula): " + ex.Message);
            }
        }

        protected void ddlIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigo = ddlIdiomas.SelectedValue;

            var nuevoIdioma = GestorIdioma.Instancia.CargarIdioma(codigo);
            GestorIdioma.Instancia.CambiarIdioma(nuevoIdioma);

            ActualizarIdioma(nuevoIdioma);
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.LibroOPelicula,
                    Operacion = TipoOperacion.CambioIdioma
                };

                bllBitacora.Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }



            // (Opcional) Mostrar mensaje visual de confirmación            
            string mensaje = codigo == "ES" ? "Idioma cambiado a Español 🇪🇸" : "Language changed to English 🇺🇸";
            ScriptManager.RegisterStartupScript(this, GetType(), "IdiomaCambiado",
                $"alert('{mensaje}');", true);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
