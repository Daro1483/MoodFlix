using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using Services;
using BLL;

namespace Moodflix
{
    public partial class Login : System.Web.UI.Page, IIdiomaObserver
    {
        private GestorIdioma gestorIdioma;

        BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();
        BLL.Pelicula bllPelicula = new BLL.Pelicula();
        BLL.Libro bllLibro = new BLL.Libro();
        BLL.Emocion bllEmocion = new BLL.Emocion();
        BLL.UsuarioEstadoBLL bllUsuarioEstado = new BLL.UsuarioEstadoBLL();
        UsuarioRolBLL usuarioRolBLL = new UsuarioRolBLL();
        RolBLL rolBLL = new RolBLL();

        // =============================================================
        // PAGE LOAD
        // =============================================================
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ddlIdiomas.Items.Count == 0)
                {
                    ddlIdiomas.Items.Add(new ListItem("Español", "ES"));
                    ddlIdiomas.Items.Add(new ListItem("English", "EN"));
                }

                var idiomaActual = GestorIdioma.Instancia.ObtenerIdiomaActual();
                if (idiomaActual != null)
                {
                    ddlIdiomas.SelectedValue = idiomaActual.Codigo;
                    ActualizarIdioma(idiomaActual);
                }
                else
                {
                    var idiomaES = GestorIdioma.Instancia.CargarIdioma("ES");
                    GestorIdioma.Instancia.CambiarIdioma(idiomaES);
                }
            }

            GestorIdioma.Instancia.Suscribir(this);
        }

        // =============================================================
        // MULTI IDIOMA
        // =============================================================
        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma == null) return;

            try
            {
                lblEmail.InnerText = idioma.Traducciones["Login_Email"];
                lblContraseña.InnerText = idioma.Traducciones["Login_Contraseña"];
                Button1.Text = idioma.Traducciones["Login_BotonIngresar"];
                LinkInitRegistro.Text = idioma.Traducciones["Login_LinkRegistro"];
            }
            catch { }
        }

        protected void ddlIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigo = ddlIdiomas.SelectedValue;
            var nuevoIdioma = GestorIdioma.Instancia.CargarIdioma(codigo);
            GestorIdioma.Instancia.CambiarIdioma(nuevoIdioma);

            string mensaje = (codigo == "ES")
                ? "Idioma cambiado a Español"
                : "Language changed to English";

            ScriptManager.RegisterStartupScript(this, GetType(),
                "IdiomaCambiado", $"alert('{mensaje}');", true);
        }

        // =============================================================
        // LOGIN
        // =============================================================
        protected void Button1_OnClick(object sender, EventArgs e)
        {
            string email = txtEmail.Value;
            string password = txtPassword.Value;

            int idUsuarioCheck = bllUsuario.GetIdByEmail(email);

            // ===========================================
            // VALIDACIÓN DE USUARIO BLOQUEADO
            // ===========================================
            if (idUsuarioCheck > 0)
            {
                var estado = bllUsuarioEstado.Obtener(idUsuarioCheck);

                if (estado != null && estado.Bloqueado)
                {
                    lblErrorMessage.Text = "El usuario está bloqueado. Contacte al webmaster.";
                    pnlErrorMessage.Visible = true;
                    return;
                }
            }

            BE.Usuario user = new BE.Usuario() { Email = email, Password = password };

            try
            {
                // ===========================================
                // VALIDAR CREDENCIALES
                // ===========================================
                if (!bllUsuario.ValidarUsuario(user))
                    throw new LoginException(LoginResult.InvalidPassword);

                // Login OK → reiniciar intentos
                bllUsuarioEstado.ReiniciarIntentos(idUsuarioCheck);

                BE.Usuario savedUser = bllUsuario.GetUser(user.Email);
                Session["Usuario"] = savedUser;
                FormsAuthentication.SetAuthCookie(savedUser.Username, false);

                // =============================================================
                // VERIFICACIÓN INTEGRIDAD BD
                // =============================================================
                List<RegistroInvalido> registrosInvalidos = bllDvh.ValidarDigitoVerificador();
                List<ColumnaInvalida> columnasInvalidas = bllDvv.ValidarDigitoVerificador();

                if (registrosInvalidos.Count > 0 || columnasInvalidas.Count > 0)
                {
                    var relaciones = usuarioRolBLL.ObtenerRolesDeUsuario(savedUser.ID);

                    List<Rol> rolesReales = relaciones
                        .Select(r => rolBLL.GetById(r.IdRol))
                        .Where(r => r != null).ToList();

                    bool esWebmaster =
                        savedUser.Username.Equals("webmaster", StringComparison.OrdinalIgnoreCase) ||
                        rolesReales.Any(r => r.Nombre.ToUpper() == "WEBMASTER");

                    bool esCliente = rolesReales.Any(r => r.Nombre.ToUpper() == "CLIENTE");
                    bool esAdminComun = !esWebmaster && !esCliente;

                    var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();

                    // ========== WEBMASTER ==========
                    // ========== WEBMASTER ==========
                    if (esWebmaster)
                    {
                     

                        // =====================================================
                        // REGISTROS AFECTADOS (DVH)
                        // =====================================================
                        string textoEncabezadoError = idioma.Traducciones.ContainsKey("Integridad_DetalleError")
                            ? idioma.Traducciones["Integridad_DetalleError"]
                            : "Detalle del error:";

                        lblInformacionRegistros.Text = "<u>" + textoEncabezadoError + "</u><br/>";

                        // Agrupar registros inválidos por tabla
                        var registrosPorTabla = registrosInvalidos
                            .GroupBy(r => r.DVH.Tabla)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(r => new
                                {
                                    Registro = r.DVH.Registro,
                                    Estado = r.Estado
                                }).ToList()
                            );

                        foreach (var tabla in registrosPorTabla)
                        {
                            lblInformacionRegistros.Text += $"<br/>Tabla {tabla.Key}<br/>";

                            foreach (var reg in tabla.Value)
                            {
                                lblInformacionRegistros.Text +=
                                    $"Registro {reg.Registro}: {reg.Estado}<br/>";
                            }
                        }

                        // =====================================================
                        // COLUMNAS AFECTADAS (DVV)
                        // =====================================================
                        lblInformacionRegistros.Text += "<br/>";

                        string textoColumnas = idioma.Traducciones.ContainsKey("Integridad_DetalleColumnas")
                            ? idioma.Traducciones["Integridad_DetalleColumnas"]
                            : "Columnas afectadas:";

                        lblInformacionColumnas.Text = "<u>" + textoColumnas + "</u><br/>";

                        // Agrupar columnas inválidas por tabla
                        var columnasPorTabla = columnasInvalidas
                            .GroupBy(c => c.DVV.Tabla)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(c => new
                                {
                                    Columna = c.DVV.Columna,
                                    Estado = c.Estado
                                }).ToList()
                            );

                        foreach (var tabla in columnasPorTabla)
                        {
                            lblInformacionColumnas.Text += $"<br/>Tabla {tabla.Key}<br/>";

                            // Obtener registros afectados para esta tabla
                            string registrosAfectados = registrosPorTabla.ContainsKey(tabla.Key)
                                ? string.Join(", ", registrosPorTabla[tabla.Key].Select(r => r.Registro))
                                : "no identificados";

                            foreach (var col in tabla.Value)
                            {
                                lblInformacionColumnas.Text +=
                                    $"Columna {col.Columna}: {col.Estado}<br/>";
                            }

                        }

                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "ShowModal", "$('#modalInconsistenciaBD').modal('show');", true);

                        return;
                    }


                    // ========== CLIENTE ==========
                    if (esCliente)
                    {
                        string msg = idioma.Traducciones["Login_ErrorBase_Cliente"];
                        ScriptManager.RegisterStartupScript(this, GetType(), "ClienteError",
                            $"alert('{msg}');", true);
                        FormsAuthentication.SignOut();
                        return;
                    }

                    // ========== ADMIN ==========
                    if (esAdminComun)
                    {
                        string msg = idioma.Traducciones["Login_ErrorBase_Admin"];
                        ScriptManager.RegisterStartupScript(this, GetType(), "AdminError",
                            $"alert('{msg}');", true);

                        FormsAuthentication.SignOut();
                        return;
                    }
                }

                // =============================================================
                // LOGIN OK FINAL (BD íntegra)
                // =============================================================
                if (registrosInvalidos.Count == 0 && columnasInvalidas.Count == 0)
                {
                    Services.Bitacora bit = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = savedUser,
                        Modulo = TipoModulo.InicioSesion,
                        Operacion = TipoOperacion.Login
                    };

                    bllBitacora.Insertar(bit);

                    bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                    bllDvv.Recalcular();

                    Response.Redirect("Emociones.aspx");
                    return;
                }
            }
            catch (LoginException)
            {
                // =============================================================
                // MANEJO DE INTENTOS FALLIDOS (LÓGICA FINAL)
                // =============================================================
                int id = bllUsuario.GetIdByEmail(email);

                // Caso: usuario NO existe
                if (id <= 0)
                {
                    lblErrorMessage.Text = "Credenciales inválidas.";
                    pnlErrorMessage.Visible = true;
                    return;
                }

                // Caso: usuario existe → registrar intento
                bllUsuarioEstado.RegistrarIntentoFallido(id);
                var estado = bllUsuarioEstado.Obtener(id);

                if (estado.Bloqueado)
                {
                    lblErrorMessage.Text = "El usuario ha sido bloqueado por múltiples intentos fallidos.";
                }
                else
                {
                    lblErrorMessage.Text = $"Contraseña incorrecta. Intento {estado.Intentos} de 3.";
                }

                pnlErrorMessage.Visible = true;
            }
        }

        // =============================================================
        // BOTÓN RECOMPONER DV
        // =============================================================
        protected void btnRecomponerDV_Click(object sender, EventArgs e)
        {
            var peliculas = bllPelicula.Listar();
            var emociones = bllEmocion.Listar();
            var libros = bllLibro.Listar();
            var usuarios = bllUsuario.Listar();
            var bitacoras = bllBitacora.Listar();

            bllDvh.LimpiarRegistrosOrfandades(peliculas, "PELICULA");
            bllDvh.LimpiarRegistrosOrfandades(emociones, "EMOCION");
            bllDvh.LimpiarRegistrosOrfandades(libros, "LIBRO");
            bllDvh.LimpiarRegistrosOrfandades(usuarios, "USUARIO");
            bllDvh.LimpiarRegistrosOrfandades(bitacoras, "BITACORA");

            var dvhs = bllDvh.Listar();

            bllDvh.Recalcular(dvhs, peliculas);
            bllDvh.Recalcular(dvhs, emociones);
            bllDvh.Recalcular(dvhs, libros);
            bllDvh.Recalcular(dvhs, usuarios);
            bllDvh.Recalcular(dvhs, bitacoras);

            bllDvv.Recalcular();

            ScriptManager.RegisterStartupScript(this, GetType(),
                "DVOK", "alert('Recalculo completado correctamente.');", true);
        }

        // =============================================================
        // RESTAURAR BACKUP
        // =============================================================
        protected void btnRestaurarBackup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Backup.aspx?modo=restauracion");
        }

        // =============================================================
        // NAVEGACIÓN
        // =============================================================
        protected void LinkInitRegistro_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Registrarse.aspx");
        }

        protected void ImageButton1_OnClick(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Emociones.aspx");
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
