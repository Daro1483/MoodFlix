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
    public partial class Bitacora : System.Web.UI.Page, IIdiomaObserver
    {
        BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Observer
            GestorIdioma.Instancia.Suscribir(this);

            // 2) Tomar usuario
            var usuario = Session["Usuario"] as BE.Usuario;

            // 3) Validar permisos
            if (usuario != null && !bllUsuario.UsuarioTienePatente(usuario, "ACCESO_BITACORA"))
            {
                Response.Redirect("~/SinPermisos.aspx");
                return;
            }

            if (!IsPostBack)
            {
                AplicarTraducciones();
                CargarCombos();
                Listar();
            }
        }

        protected void btnFiltrar_OnClick(object sender, EventArgs e)
        {
            // FECHAS
            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            if (!string.IsNullOrWhiteSpace(txtFechaHoraInicio.Value))
            {
                if (DateTime.TryParse(txtFechaHoraInicio.Value, out DateTime f1))
                    fechaInicio = f1;
            }

            if (!string.IsNullOrWhiteSpace(txtFechaHoraFin.Value))
            {
                if (DateTime.TryParse(txtFechaHoraFin.Value, out DateTime f2))
                    fechaFin = f2;
            }

            // VALIDACIÓN DE RANGO
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alerta3",
                    "alert('La Fecha de Fin no puede ser menor a la Fecha de Inicio.');", true);
                return;
            }

            // USUARIO
            string email = ddlUsuariosFiltro.SelectedValue;

            // MÓDULO
            string modulo = ddlModulosFiltro.SelectedValue;

            // LLAMADA A MP/BLL – FILTRADO AVANZADO
            var resultado = bllBitacora.FiltrarAvanzado(
                fechaInicio,
                fechaFin,
                email,
                modulo
            );

            gvBitacora.DataSource = resultado;
            gvBitacora.DataBind();

            // BITÁCORA
            var usuario = Session["Usuario"] as BE.Usuario;
            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Bitacora,
                    Operacion = TipoOperacion.Filtrar
                };

                bllBitacora.Insertar(bitacora);

                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }
        }


        private void CargarCombos()
        {
            // USUARIOS
            ddlUsuariosFiltro.DataSource = bllUsuario.Listar();
            ddlUsuariosFiltro.DataTextField = "Email";
            ddlUsuariosFiltro.DataValueField = "Email";
            ddlUsuariosFiltro.DataBind();
            ddlUsuariosFiltro.Items.Insert(0, new ListItem("Todos", ""));

            // MÓDULOS
            ddlModulosFiltro.DataSource = Enum.GetNames(typeof(TipoModulo));
            ddlModulosFiltro.DataBind();
            ddlModulosFiltro.Items.Insert(0, new ListItem("Todos", ""));
        }



        public void ActualizarIdioma(Idioma idioma)
        {
            AplicarTraducciones();
            Listar();
        }

        private void AplicarTraducciones()
        {
            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
            if (idioma == null) return;

            lblTitulo.InnerText = idioma.Traducciones["Bitacora_lblTitulo"];
            lblFechaHoraInicio.InnerText = idioma.Traducciones["Bitacora_lblFechaHoraInicio"];
            lblFechaHoraFin.InnerText = idioma.Traducciones["Bitacora_lblFechaHoraFin"];
            btnFiltrar.Text = idioma.Traducciones["Bitacora_btnFiltrar"];
            btnLimpiar.Text = idioma.Traducciones["Bitacora_btnLimpiar"];
            lblUsuarioFiltro.InnerText = idioma.Traducciones["Bitacora_lblUsuarioFiltro"];
            lblModuloFiltro.InnerText = idioma.Traducciones["Bitacora_lblModuloFiltro"];
            gvBitacora.Columns[0].HeaderText = idioma.Traducciones["Bitacora_grd_ID"];
            gvBitacora.Columns[1].HeaderText = idioma.Traducciones["Bitacora_grd_Fecha"];
            gvBitacora.Columns[2].HeaderText = idioma.Traducciones["Bitacora_grd_EmailUsuario"];
            gvBitacora.Columns[3].HeaderText = idioma.Traducciones["Bitacora_grd_Modulo"];
            gvBitacora.Columns[4].HeaderText = idioma.Traducciones["Bitacora_grd_Operacion"];
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            // Limpiar fechas
            txtFechaHoraInicio.Value = "";
            txtFechaHoraFin.Value = "";

            // Reiniciar combos a “Todos”
            ddlUsuariosFiltro.SelectedIndex = 0;
            ddlModulosFiltro.SelectedIndex = 0;

            // Listar todo
            Listar();
        }


        public void Listar()
        {
            gvBitacora.DataSource = bllBitacora.Listar();
            gvBitacora.DataBind();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
