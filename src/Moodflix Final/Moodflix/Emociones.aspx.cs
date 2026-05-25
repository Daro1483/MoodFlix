    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using BE;
    using Services;

    namespace Moodflix
    {
        public partial class Emociones : System.Web.UI.Page, IIdiomaObserver
        {
            BLL.DVH bllDvh = new BLL.DVH();
            BLL.DVV bllDvv = new BLL.DVV();
            BLL.Usuario  bllUsuario = new BLL.Usuario();
            BLL.Pelicula bllPelicula = new BLL.Pelicula();
            BLL.Libro bllLibro = new BLL.Libro();
            BLL.Bitacora bllBitacora = new BLL.Bitacora();
        
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    // ... ya existente ...
                    GestorIdioma.Instancia.Suscribir(this);

                    // 🔹 Mantener idioma actual en el combo
                    var idiomaActual = GestorIdioma.Instancia.ObtenerIdiomaActual();
                    if (idiomaActual != null)
                        ddlIdiomas.SelectedValue = idiomaActual.Codigo;
                    else
                        ddlIdiomas.SelectedValue = "ES"; // por defecto

                    ActualizarIdioma(idiomaActual);

                    // 🔹 Tu código original
                    SetNavbar();
                    Session["Emociones"] = bllEmocion.Listar();
                    //bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                }

                GenerateCards();
            }

        void SetNavbar()
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

            bool accBitacora = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_BITACORA");
            bool accSeguridad = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_SEGURIDAD");
            bool accPerfiles = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_PERFILES");
            bool accABM = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_ABM");
            bool accUsuariosBloqueados = accSeguridad;

            bool tieneHerramientas = accBitacora || accSeguridad || accPerfiles || accABM || accUsuariosBloqueados;
            PlantillaHerramientas.Visible = tieneHerramientas;

            // 🔥 IMPORTANTE: SOLO ARMAR EL DDL EN PRIMERA CARGA
            if (!IsPostBack)
            {
                ddlHerramientas.Items.Clear();
                ddlHerramientas.Items.Add(new ListItem("Herramientas", ""));

                if (accBitacora)
                    ddlHerramientas.Items.Add(new ListItem("Bitácora", "Bitacora"));

                if (accSeguridad)
                    ddlHerramientas.Items.Add(new ListItem("Seguridad", "Seguridad"));

                if (accPerfiles)
                    ddlHerramientas.Items.Add(new ListItem("Gestión de Perfiles", "Gestión de Perfiles"));

                if (accABM)
                    ddlHerramientas.Items.Add(new ListItem("ABM", "ABM"));

                if (accUsuariosBloqueados)
                    ddlHerramientas.Items.Add(new ListItem("Usuarios bloqueados", "UsuariosBloqueados"));
            }
        }





        BLL.Emocion bllEmocion = new BLL.Emocion();
            public void GenerateCards()
            {
                List<BE.Emocion> emociones = Session["Emociones"] as List<BE.Emocion>;


                foreach (var emocion in emociones)
                {
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "col-6  col-md-4 col-lg-3 text-center");

                    ImageButton btn = new ImageButton();
                    btn.ImageUrl = emocion.Uri;
                    btn.Width = Unit.Pixel(210);
                    btn.Click += btnEmocion1_OnClick;

                    HtmlGenericControl h3 = new HtmlGenericControl("h3");
                    h3.Attributes.Add("class", "w-100");
                    h3.InnerText = emocion.TipoEmocion.ToString();

                    div.Controls.Add(btn);
                    div.Controls.Add(h3);


                    emotionsContainer.Controls.Add(div);
                }


            }



            protected void OnClick(object sender, ImageClickEventArgs e)
            {
                Response.Redirect("Emociones.aspx");
            }

            protected void LinkInitRegistro_OnClick(object sender, EventArgs e)
            {
                Response.Redirect("Registrarse.aspx");
            }

            protected void LinkLogout_OnClick(object sender, EventArgs e)
            {
                FormsAuthentication.SignOut();

                Services.Bitacora bitacora = new Services.Bitacora();
                bitacora.User = bllUsuario.GetUserByUsername(HttpContext.Current.User.Identity.Name);
                bitacora.Fecha = DateTime.Now;
                bitacora.Operacion = TipoOperacion.Logout;
                bitacora.Modulo = (TipoModulo)Enum.Parse(typeof(TipoModulo), "Emociones");

                bllBitacora.Insertar(bitacora);
                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
                Session.Clear();
                Response.Redirect("Login.aspx");
            }


            protected void btnEmocion1_OnClick(object sender, ImageClickEventArgs e)
            {
                ImageButton btn = sender as ImageButton;
                string path = btn.ImageUrl;

                string emocion = StringManager.ExtraerPalabraAntesPng(path);
                Session["Emocion"] = emocion;

                Response.Redirect("LibroOPelicula.aspx");
            }

            protected void linkInitLogin_OnClick(object sender, EventArgs e)
            {
                Response.Redirect("Login.aspx");
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
                    case "Gestión de Perfiles":
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
            public void ActualizarIdioma(Idioma idioma)
            {
                if (idioma == null || idioma.Traducciones == null) return;

                linkInitLogin.Text = idioma.Traducciones["Emociones_LinkInicioSesion"];
                LinkInitRegistro.Text = idioma.Traducciones["Emociones_LinkRegistro"];
                LinkLogout.Text = idioma.Traducciones["Emociones_LinkCierreSesion"];
                lblTitulo.Text = idioma.Traducciones["Emociones_Titulo"];

                // Traducción del combo unificado
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
                        case "Gestión de Perfiles":
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

            protected void Page_Unload(object sender, EventArgs e)
            {
                GestorIdioma.Instancia.Desuscribir(this);
            }

            protected void ddlIdiomas_SelectedIndexChanged(object sender, EventArgs e)
            {
                string codigo = ddlIdiomas.SelectedValue;
                var nuevoIdioma = GestorIdioma.Instancia.CargarIdioma(codigo);
                GestorIdioma.Instancia.CambiarIdioma(nuevoIdioma);

                // 🔸 Actualizar los textos en la misma página inmediatamente
                ActualizarIdioma(nuevoIdioma);

            //registro en bitacora el cambio de idioma
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Emociones,
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

        }
    }
