using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;
using Services;

namespace Moodflix
{
    public partial class Perfiles : System.Web.UI.Page, IIdiomaObserver
    {
        private readonly RolBLL rolBLL = new RolBLL();
        private readonly RolHijoBLL rolHijoBLL = new RolHijoBLL();

        // NUEVO: ASIGNACIÓN DE FAMILIAS A USUARIOS
        private readonly BLL.Usuario usuarioBLL = new BLL.Usuario();
        private readonly UsuarioRolBLL usuarioRolBLL = new UsuarioRolBLL();

        //DV
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();

        //Bitacora
        BLL.Bitacora bllBitacora = new BLL.Bitacora();


        // ============================================================
        //   PAGE LOAD
        // ============================================================
        protected void Page_Load(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Suscribir(this);

            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var usuario = (BE.Usuario)Session["Usuario"];

            if (!new BLL.Usuario().UsuarioTienePatente(usuario, "ACCESO_PERFILES"))
            {
                Response.Redirect("~/SinPermisos.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarPatentes();
                CargarFamilias();

                CargarUsuarios();
                CargarFamiliasDisponibles();
                lstFamiliasUsuario.Items.Clear();

                AplicarTraducciones();
            }
        }


        // ============================================================
        //   TRADUCCIONES
        // ============================================================
        public void ActualizarIdioma(Idioma idioma)
        {
            AplicarTraducciones();
        }

        private void AplicarTraducciones()
        {
            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
            if (idioma == null) return;

            try
            {
                tituloPagina.InnerText = idioma.Traducciones["Perfiles_lblTitulo"];
                lblRolesDisponibles.Text = idioma.Traducciones["Perfiles_lblRoles"];
                lblFamilias.Text = idioma.Traducciones["Perfiles_lblFamilias"];
                lblRolesFamilia.Text = idioma.Traducciones["Perfiles_lblRolesFamilia"];
                lblNuevaFamilia.Text = idioma.Traducciones["Perfiles_lblNuevaFamilia"];

                btnCrearFamilia.Text = idioma.Traducciones["Perfiles_btnCrearFamilia"];
                btnEliminarFamilia.Text = idioma.Traducciones["Perfiles_btnEliminarFamilia"];
                btnAgregarRol.Text = idioma.Traducciones["Perfiles_btnAgregarRol"];
                btnQuitarRol.Text = idioma.Traducciones["Perfiles_btnQuitarRol"];

                lblSeleccionUsuario.Text =
                    idioma.Traducciones.ContainsKey("Perfiles_lblSeleccionUsuario")
                    ? idioma.Traducciones["Perfiles_lblSeleccionUsuario"]
                    : "Seleccionar usuario";

                lblFamiliasDisponibles.Text =
                    idioma.Traducciones.ContainsKey("Perfiles_lblFamiliasDisponibles")
                    ? idioma.Traducciones["Perfiles_lblFamiliasDisponibles"]
                    : "Familias disponibles";

                lblFamiliasUsuario.Text =
                    idioma.Traducciones.ContainsKey("Perfiles_lblFamiliasUsuario")
                    ? idioma.Traducciones["Perfiles_lblFamiliasUsuario"]
                    : "Familias asignadas al usuario";

                btnAsignarFamilia.Text =
                    idioma.Traducciones.ContainsKey("Perfiles_btnAsignarFamilia")
                    ? idioma.Traducciones["Perfiles_btnAsignarFamilia"]
                    : "Asignar familia";

                btnQuitarFamilia.Text =
                    idioma.Traducciones.ContainsKey("Perfiles_btnQuitarFamilia")
                    ? idioma.Traducciones["Perfiles_btnQuitarFamilia"]
                    : "Quitar familia";
            }
            catch { }
        }



        // ============================================================
        //  BLOQUE SUPERIOR — PATENTES / FAMILIAS
        // ============================================================
        private void CargarPatentes()
        {
            lstRolesDisponibles.DataSource = rolBLL.GetPatentes();
            lstRolesDisponibles.DataTextField = "Nombre";
            lstRolesDisponibles.DataValueField = "IdRol";
            lstRolesDisponibles.DataBind();
        }

        private void CargarFamilias()
        {
            lstFamilias.DataSource = rolBLL.GetFamilias();
            lstFamilias.DataTextField = "Nombre";
            lstFamilias.DataValueField = "IdRol";
            lstFamilias.DataBind();
        }

        private void CargarRolesDeFamilia(int idFamilia)
        {
            lstRolesFamilia.Items.Clear();

            var hijos = rolHijoBLL.ObtenerHijos(idFamilia);
            foreach (var h in hijos)
            {
                Rol rolHijo = rolBLL.GetById(h.IdRolHijo);
                if (rolHijo != null)
                {
                    lstRolesFamilia.Items.Add(new ListItem(
                        rolHijo.Nombre,
                        rolHijo.IdRol.ToString()
                    ));
                }
            }

            FiltrarRolesDisponibles(idFamilia);
        }

        private void FiltrarRolesDisponibles(int idFamilia)
        {
            var hijos = rolHijoBLL.ObtenerHijos(idFamilia);
            var asignados = hijos.Select(h => h.IdRolHijo).ToHashSet();

            for (int i = lstRolesDisponibles.Items.Count - 1; i >= 0; i--)
            {
                int idRol = int.Parse(lstRolesDisponibles.Items[i].Value);
                if (asignados.Contains(idRol))
                    lstRolesDisponibles.Items.RemoveAt(i);
            }
        }



        // ============================================================
        //   EVENTOS BLOQUE SUPERIOR
        // ============================================================
        protected void btnCrearFamilia_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNuevaFamilia.Text)) return;

            string nombreFam = txtNuevaFamilia.Text.Trim();
            rolBLL.CrearFamilia(nombreFam);
            txtNuevaFamilia.Text = "";

            CargarFamilias();

            // BITÁCORA
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Perfiles,
                    Operacion = TipoOperacion.CrearFamilia,
                };

                new BLL.Bitacora().Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }

            // ALERTA
            ScriptManager.RegisterStartupScript(
                this, GetType(), "alertCrearFamilia",
                $"alert('La familia \"{nombreFam}\" fue creada correctamente.');", true);

            //ACTUALIZAR LISTBOX
            CargarFamiliasDisponibles();
        }


        protected void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            if (lstFamilias.SelectedValue == "")
                return;

            int idFamilia = int.Parse(lstFamilias.SelectedValue);
            string nombreFamilia = lstFamilias.SelectedItem.Text;

            // 1) Validar que NO tenga usuarios asignados
            var usuariosConFamilia = usuarioRolBLL.ObtenerUsuariosPorRol(idFamilia);
            if (usuariosConFamilia.Count > 0)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "alertFamUsuarios",
                    $"alert('No se puede eliminar la familia \"{nombreFamilia}\" porque tiene usuarios asignados.');",
                    true);
                return;
            }

            // 2) Eliminar hijos (roles asignados)
            rolHijoBLL.EliminarHijosDeFamilia(idFamilia);

            // 3) Eliminar la familia
            rolBLL.EliminarFamilia(idFamilia);

            // 4) Refrescar UI
            CargarFamilias();
            lstRolesFamilia.Items.Clear();
            CargarPatentes();

            // 5) Bitácora
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Perfiles,
                    Operacion = TipoOperacion.EliminarFamilia
                };

                new BLL.Bitacora().Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }

            // 6) Alerta
            ScriptManager.RegisterStartupScript(
                this, GetType(), "alertFamEliminada",
                $"alert('La familia \"{nombreFamilia}\" fue eliminada correctamente.');",
                true);
            
            //ACTUALIZAR LISTBOX
            CargarFamiliasDisponibles();
        }

        protected void lstFamilias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFamilias.SelectedValue == "") return;

            int idFam = int.Parse(lstFamilias.SelectedValue);

            CargarPatentes();
            CargarRolesDeFamilia(idFam);
        }

        protected void btnAgregarRol_Click(object sender, EventArgs e)
        {
            if (lstFamilias.SelectedValue == "" || lstRolesDisponibles.SelectedValue == "") return;

            int idPadre = int.Parse(lstFamilias.SelectedValue);
            int idHijo = int.Parse(lstRolesDisponibles.SelectedValue);

            rolHijoBLL.InsertarRolHijo(idPadre, idHijo);

            // datos para alerta
            string fam = lstFamilias.SelectedItem.Text;
            string rol = lstRolesDisponibles.SelectedItem.Text;

            CargarPatentes();
            CargarRolesDeFamilia(idPadre);

            // BITÁCORA
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Perfiles,
                    Operacion = TipoOperacion.AgregarRol,
                };

                new BLL.Bitacora().Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }

            // ALERTA
            ScriptManager.RegisterStartupScript(
                this, GetType(), "alertAgregarRol",
                $"alert('Se agregó el rol \"{rol}\" a la familia \"{fam}\".');", true);
        }

        protected void btnQuitarRol_Click(object sender, EventArgs e)
        {
            if (lstFamilias.SelectedValue == "" || lstRolesFamilia.SelectedValue == "") return;

            int idPadre = int.Parse(lstFamilias.SelectedValue);
            int idHijo = int.Parse(lstRolesFamilia.SelectedValue);

            string fam = lstFamilias.SelectedItem.Text;
            string rol = lstRolesFamilia.SelectedItem.Text;

            rolHijoBLL.EliminarHijoPuntual(idPadre, idHijo);

            CargarPatentes();
            CargarRolesDeFamilia(idPadre);

            // BITÁCORA
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Perfiles,
                    Operacion = TipoOperacion.QuitarRol,
                };

                new BLL.Bitacora().Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }

            // ALERTA
            ScriptManager.RegisterStartupScript(
                this, GetType(), "alertQuitarRol",
                $"alert('Se quitó el rol \"{rol}\" de la familia \"{fam}\".');", true);
        }



        // ============================================================
        //   BLOQUE INFERIOR — USUARIO <-> FAMILIAS
        // ============================================================
        private void CargarUsuarios()
        {
            var usuarios = usuarioBLL.Listar();

            ddlUsuarios.DataSource = usuarios;
            ddlUsuarios.DataTextField = "Email";
            ddlUsuarios.DataValueField = "ID";
            ddlUsuarios.DataBind();

            ddlUsuarios.Items.Insert(0, new ListItem("Seleccione un usuario", "0"));
        }

        private void CargarFamiliasDisponibles()
        {
            lstFamiliasDisponibles.DataSource = rolBLL.GetFamilias();
            lstFamiliasDisponibles.DataTextField = "Nombre";
            lstFamiliasDisponibles.DataValueField = "IdRol";
            lstFamiliasDisponibles.DataBind();
        }

        private void CargarFamiliasDelUsuario(int idUsuario)
        {
            lstFamiliasUsuario.Items.Clear();

            var rolesUsuario = usuarioRolBLL.ObtenerRolesDeUsuario(idUsuario);

            foreach (var ur in rolesUsuario)
            {
                Rol rol = rolBLL.GetById(ur.IdRol);

                if (rol != null && rol.Tipo == "FAMILIA")
                {
                    lstFamiliasUsuario.Items.Add(new ListItem(
                        rol.Nombre,
                        rol.IdRol.ToString()
                    ));
                }
            }
        }

        protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(ddlUsuarios.SelectedValue);
            lstFamiliasUsuario.Items.Clear();

            if (idUsuario == 0) return;

            CargarFamiliasDelUsuario(idUsuario);
        }


        // ASIGNAR FAMILIA
        protected void btnAsignarFamilia_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(ddlUsuarios.SelectedValue);
            if (idUsuario == 0 || lstFamiliasDisponibles.SelectedValue == "") return;

            int idFamilia = int.Parse(lstFamiliasDisponibles.SelectedValue);

            usuarioRolBLL.AsignarRol(idUsuario, idFamilia);
            CargarFamiliasDelUsuario(idUsuario);

            // Datos para alerta
            string email = ddlUsuarios.SelectedItem.Text;
            string familia = lstFamiliasDisponibles.SelectedItem.Text;

            // BITÁCORA
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Perfiles,
                    Operacion = TipoOperacion.AsignarFamilia,
                };

                new BLL.Bitacora().Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }

            // ALERTA
            ScriptManager.RegisterStartupScript(
                this, GetType(), "alertAsignarFam",
                $"alert('Se asignó la familia \"{familia}\" al usuario \"{email}\".');", true);
        }

        // QUITAR FAMILIA
        protected void btnQuitarFamilia_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(ddlUsuarios.SelectedValue);
            if (idUsuario == 0 || lstFamiliasUsuario.SelectedValue == "") return;

            int idFamilia = int.Parse(lstFamiliasUsuario.SelectedValue);

            // Datos para alerta
            string email = ddlUsuarios.SelectedItem.Text;
            string familia = lstFamiliasUsuario.SelectedItem.Text;

            usuarioRolBLL.EliminarRol(idUsuario, idFamilia);

            CargarFamiliasDelUsuario(idUsuario);

            // BITÁCORA
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Perfiles,
                    Operacion = TipoOperacion.QuitarFamilia,
                };

                new BLL.Bitacora().Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }

            // ALERTA
            ScriptManager.RegisterStartupScript(
                this, GetType(), "alertQuitarFam",
                $"alert('Se quitó la familia \"{familia}\" del usuario \"{email}\".');", true);
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
