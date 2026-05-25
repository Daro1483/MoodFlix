using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Services;

namespace Moodflix
{
    public partial class UsuariosBloqueados : System.Web.UI.Page, IIdiomaObserver
    {
        UsuarioEstadoBLL usuarioEstadoBLL = new UsuarioEstadoBLL();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();

        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔹 Registrar observer SIEMPRE primero
            GestorIdioma.Instancia.Suscribir(this);

            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario == null || !usuario.Username.Equals("webmaster", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("~/SinPermisos.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarUsuarios();

                // Aplicar traducción inicial
                var idiomaActual = GestorIdioma.Instancia.ObtenerIdiomaActual();
                ActualizarIdioma(idiomaActual);
            }
        }

        private void CargarUsuarios()
        {
            gvUsuarios.DataSource = usuarioEstadoBLL.ListarUsuariosEstado();
            gvUsuarios.DataBind();
        }

        // 🔹 Implementación del patrón Observer
        public void ActualizarIdioma(BE.Idioma idioma)
        {
            if (idioma == null) return;

            // Título
            titulo.InnerText = idioma.Traducciones["UsuariosBloqueados_Titulo"];

            // Columnas
            gvUsuarios.Columns[0].HeaderText = idioma.Traducciones["UsuariosBloqueados_ColId"];
            gvUsuarios.Columns[1].HeaderText = idioma.Traducciones["UsuariosBloqueados_ColUsuario"];
            gvUsuarios.Columns[2].HeaderText = idioma.Traducciones["UsuariosBloqueados_ColEmail"];
            gvUsuarios.Columns[3].HeaderText = idioma.Traducciones["UsuariosBloqueados_ColIntentos"];
            gvUsuarios.Columns[4].HeaderText = idioma.Traducciones["UsuariosBloqueados_ColBloqueado"];
            gvUsuarios.Columns[5].HeaderText = idioma.Traducciones["UsuariosBloqueados_ColAcciones"];

            // 🔹 MUY IMPORTANTE: volver a bindear para refrescar los headers
            gvUsuarios.DataSource = usuarioEstadoBLL.ListarUsuariosEstado();
            gvUsuarios.DataBind();
        }


        protected void gvUsuarios_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idUsuario = Convert.ToInt32(e.CommandArgument);

            // Obtener la fila donde se hizo clic
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            // Buscar Username EN LA GRILLA
            // La columna de Username es la 1 (0 = ID, 1 = Usuario)
            GridViewRow fila = ((Control)e.CommandSource).NamingContainer as GridViewRow;

            string usernameAfectado = fila.Cells[1].Text; // <–– AQUÍ ESTÁ EL USERNAME

            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();

            if (e.CommandName == "Bloquear")
            {
                usuarioEstadoBLL.Bloquear(idUsuario);

                // Registrar en Bitácora
                var usuario = Session["Usuario"] as BE.Usuario;
                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.Usuarios,
                        Operacion = TipoOperacion.Bloquear,
                    };

                    bllBitacora.Insertar(bitacora);

                    // 6) DVH / DVV
                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }



                string mensaje = idioma.Traducciones["UsuariosBloqueados_AlertBloqueado"]
                                 .Replace("{USER}", usernameAfectado);

                ScriptManager.RegisterStartupScript(this, GetType(), "alertBloqueo",
                    $"alert('{mensaje}');", true);
            }

            if (e.CommandName == "Desbloquear")
            {
                usuarioEstadoBLL.Desbloquear(idUsuario);
                // Registrar en Bitácora
                var usuario = Session["Usuario"] as BE.Usuario;
                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.Usuarios,
                        Operacion = TipoOperacion.Desbloquear,
                    };

                    bllBitacora.Insertar(bitacora);

                    // 6) DVH / DVV
                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();
                }

                string mensaje = idioma.Traducciones["UsuariosBloqueados_AlertDesbloqueado"]
                                 .Replace("{USER}", usernameAfectado);

                ScriptManager.RegisterStartupScript(this, GetType(), "alertDesbloqueo",
                    $"alert('{mensaje}');", true);
            }

            CargarUsuarios();
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
                if (idioma == null) return;

                // Buscar los botones dentro del TemplateField
                Button btnBloquear = e.Row.FindControl("btnBloquear") as Button;
                Button btnDesbloquear = e.Row.FindControl("btnDesbloquear") as Button;

                if (btnBloquear != null)
                    btnBloquear.Text = idioma.Traducciones["UsuariosBloqueados_BotonBloquear"];

                if (btnDesbloquear != null)
                    btnDesbloquear.Text = idioma.Traducciones["UsuariosBloqueados_BotonDesbloquear"];
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            // 🔹 Desuscribir para evitar fugas de memoria
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
