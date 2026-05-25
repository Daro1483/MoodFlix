using Services;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Moodflix
{
    public partial class SiteMaster : MasterPage, IIdiomaObserver
    {
        private readonly BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Idioma
            GestorIdioma.Instancia.Suscribir(this);

            string paginaActual = Page.AppRelativeVirtualPath.ToLower();

            // 2) Páginas públicas
            bool esPaginaPublica =
                paginaActual.Contains("login.aspx") ||
                paginaActual.Contains("registrarse.aspx") ||
                paginaActual.Contains("sinpermisos.aspx");

            // 3) Usuario actual
            var usuario = Session["Usuario"] as BE.Usuario;

            // 4) Si NO hay sesión → mandar al login
            if (usuario == null && !esPaginaPublica)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + paginaActual);
                return;
            }

            // 5) Si HAY sesión y vino al login → mandarlo a Emociones
            if (usuario != null && paginaActual.Contains("login.aspx"))
            {
                Response.Redirect("~/Emociones.aspx");
                return;
            }

            // 6) Primera carga
            if (!IsPostBack)
            {
                var idiomaActual = GestorIdioma.Instancia.ObtenerIdiomaActual();
                ddlIdiomas.SelectedValue = idiomaActual?.Codigo ?? "ES";

                ActualizarIdioma(idiomaActual);

                if (usuario != null)
                    SetNavbar();
            }
            else
            {
                if (usuario != null)
                    SetNavbar();
            }
        }

        // ============================================================
        // NAVBAR / HERRAMIENTAS
        // ============================================================

        void SetNavbar()
        {
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario == null)
            {
                PlantillaUserAnonimo.Visible = true;
                PlantillaUserRegistrado.Visible = false;
                PlantillaWebmaster.Visible = false;
                return;
            }

            PlantillaUserAnonimo.Visible = false;
            PlantillaUserRegistrado.Visible = true;
            PlantillaIdioma.Visible = true;

            LinkProfile.Text = usuario.Username;

            // Patentes actuales
            bool accBitacora = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_BITACORA");
            bool accSeguridad = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_SEGURIDAD");
            bool accPerfiles = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_PERFILES");
            bool accABM = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_ABM");

            // NUEVA patente para Usuarios bloqueados
            bool accUsuariosBloqueados = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_SEGURIDAD");

            // Mostrar el combo solo si tiene opciones
            PlantillaWebmaster.Visible =
                accBitacora || accSeguridad || accPerfiles || accABM || accUsuariosBloqueados;

            // EJEMPLO DE ÍNDICES:
            // 0 - Herramientas (texto)
            // 1 - Bitácora
            // 2 - Seguridad (Backup)
            // 3 - Perfiles
            // 4 - ABM
            // 5 - Usuarios bloqueados  ← NUEVO

            if (ddlWebmaster.Items.Count == 5)
            {
                // Agregar la opción solo una vez
                ddlWebmaster.Items.Add(new ListItem("Usuarios bloqueados", "UsuariosBloqueados"));
            }

            if (ddlWebmaster.Items.Count >= 6)
            {
                ddlWebmaster.Items[1].Enabled = accBitacora;
                ddlWebmaster.Items[2].Enabled = accSeguridad;
                ddlWebmaster.Items[3].Enabled = accPerfiles;
                ddlWebmaster.Items[4].Enabled = accABM;
                ddlWebmaster.Items[5].Enabled = accUsuariosBloqueados; // NUEVA
            }
        }

        protected void ddlActions_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string selectedValue = ddl.SelectedValue;

            switch (selectedValue)
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

                default:
                    break;
            }
        }

        // ============================================================
        // NAVEGACIÓN BÁSICA
        // ============================================================

        protected void linkEmociones_OnClick(object sender, EventArgs e) => Response.Redirect("Emociones.aspx");
        protected void linkLibros_OnClick(object sender, EventArgs e) => Response.Redirect("Libros.aspx");
        protected void linkPeliculas_OnClick(object sender, EventArgs e) => Response.Redirect("Peliculas.aspx");
        protected void linkLogin_OnClick(object sender, EventArgs e) => Response.Redirect("Login.aspx");
        protected void linkRegistro_OnClick(object sender, EventArgs e) => Response.Redirect("Registrarse.aspx");
        protected void linkMiscompras_OnClick(object sender, EventArgs e) => Response.Redirect("Carrito.aspx");

        protected void LinkLogout_OnClick(object sender, EventArgs e)
        {
            var usuario = Session["Usuario"] as BE.Usuario;

            try
            {
                if (usuario != null)
                {
                    var bllBitacora = new BLL.Bitacora();
                    var bllDvh = new BLL.DVH();
                    var bllDvv = new BLL.DVV();

                    var bitacora = new Services.Bitacora
                    {
                        User = usuario,
                        Fecha = DateTime.Now,
                        Operacion = TipoOperacion.Logout,
                        Modulo = TipoModulo.InicioSesion
                    };

                    bllBitacora.Insertar(bitacora);
                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }
            }
            catch { }

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect("~/Login.aspx");
        }

        // ============================================================
        // MULTI-IDIOMA
        // ============================================================

        public void ActualizarIdioma(BE.Idioma idioma)
        {
            if (idioma == null || idioma.Traducciones == null) return;

            linkLogin.Text = idioma.Traducciones["Emociones_LinkInicioSesion"];
            linkRegistro.Text = idioma.Traducciones["Emociones_LinkRegistro"];
            LinkLogout.Text = idioma.Traducciones["Emociones_LinkCierreSesion"];
            txtMisCompras.InnerText = idioma.Traducciones["SiteMaster_LinkMisCompras"];
            linkEmociones.Text = idioma.Traducciones["SiteMaster_LinkEmociones"];
            linkLibros.Text = idioma.Traducciones["SiteMaster_LinkLibros"];
            linkPeliculas.Text = idioma.Traducciones["SiteMaster_LinkPeliculas"];

            ddlWebmaster.Items[0].Text = idioma.Traducciones["Emociones_Herramientas"];
            ddlWebmaster.Items[1].Text = idioma.Traducciones["Emociones_Bitacora"];
            ddlWebmaster.Items[2].Text = idioma.Traducciones["SiteMaster_LinkSeguridad"];
            ddlWebmaster.Items[3].Text = idioma.Traducciones["SiteMaster_LinkPerfiles"];
            ddlWebmaster.Items[4].Text = idioma.Traducciones["Emociones_ABM"];
            ddlWebmaster.Items[5].Text = idioma.Traducciones["SiteMaster_LinkUsuariosBloqueados"];
            
        }

        protected void ddlIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigo = ddlIdiomas.SelectedValue;
            var nuevoIdioma = GestorIdioma.Instancia.CargarIdioma(codigo);
            GestorIdioma.Instancia.CambiarIdioma(nuevoIdioma);

            // 🔹 Bitácora: cambio de idioma
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                TipoModulo modulo = TipoModulo.Desconocido;
                string pagina = ObtenerNombreDePagina().ToLower();

                switch (pagina)
                {
                    case "peliculas": modulo = TipoModulo.Peliculas; break;
                    case "libros": modulo = TipoModulo.Libros; break;
                    case "emociones": modulo = TipoModulo.Emociones; break;
                    case "bitacora": modulo = TipoModulo.Bitacora; break;
                    case "backup": modulo = TipoModulo.Backup; break;
                    case "perfiles": modulo = TipoModulo.Perfiles; break;
                    case "abm": modulo = TipoModulo.ABM; break;
                    case "usuariosbloqueados": modulo = TipoModulo.Usuarios; break;
                }

                var bllBitacora = new BLL.Bitacora();
                var bitacora = new Services.Bitacora
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = modulo,
                    Operacion = TipoOperacion.CambioIdioma
                };

                bllBitacora.Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();

                string mensaje = codigo == "ES" ? "Idioma cambiado a Español 🇪🇸" : "Language changed to English 🇺🇸";
                ScriptManager.RegisterStartupScript(this, GetType(), "IdiomaCambiado",
                    $"alert('{mensaje}');", true);
            }
        }


        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }

        public string ObtenerNombreDePagina()
        {
            string url = Request.Url.AbsolutePath;
            string nombre = new System.IO.FileInfo(url).Name;
            return nombre.EndsWith(".aspx") ? nombre.Substring(0, nombre.Length - 5) : nombre;
        }
    }
}
