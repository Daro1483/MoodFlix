using System;
using System.Web.UI;
using Services;
using System.IO;
using BE;
using System.Web;

namespace Moodflix
{
    public partial class Backup : System.Web.UI.Page, IIdiomaObserver
    {
        BLL.BackupService bllBackupService = new BLL.BackupService();
        BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Suscribirse SIEMPRE al idioma
            GestorIdioma.Instancia.Suscribir(this);

            // =============================================
            // 1) VERIFICAR MODO DE ACCESO
            // =============================================
            bool modoRestauracion = Request.QueryString["modo"] == "restauracion";

            if (!modoRestauracion)
            {
                // ACCESO NORMAL
                if (Session["Usuario"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                var usuario = (BE.Usuario)Session["Usuario"];

                bool tiene = bllUsuario.UsuarioTienePatente(usuario, "ACCESO_SEGURIDAD");

                if (!tiene)
                {
                    Response.Redirect("~/SinPermisos.aspx");
                    return;
                }
            }
            else
            {
                // ACCESO ESPECIAL (SOLO CUANDO LA BD ESTÁ CORRUPTA)
                // No validamos Session ni patentes
            }

            if (!IsPostBack)
                AplicarTraducciones();
        }

        // ===================================================
        // MULTIIDIOMA
        // ===================================================
        public void ActualizarIdioma(Idioma idioma)
        {
            AplicarTraducciones();
        }

        private void AplicarTraducciones()
        {
            var idioma = GestorIdioma.Instancia.ObtenerIdiomaActual();
            if (idioma == null) return;

            lblTitulo.InnerText = idioma.Traducciones["Backup_lblTitulo"];
            lblGenerarBackup.InnerText = idioma.Traducciones["Backup_lblGenerar"];
            btnGenerarBackup.Text = idioma.Traducciones["Backup_BtnGenerar"];
            lblHacerRestore.InnerText = idioma.Traducciones["Backup_lblHacerRestauracion"];
            lblRutaRestore.InnerText = idioma.Traducciones["Backup_lblRutaRestauracion"];
            lblExaminar.InnerText = idioma.Traducciones["Backup_btnExaminar"];
            btnComenzarRestore.Text = idioma.Traducciones["Backup_btnComenzar"];
        }

        // ===================================================
        // GENERAR BACKUP
        // ===================================================
        protected void btnGenerarBackup_OnClick(object sender, EventArgs e)
        {
            string carpetaFisica = @"C:\BackupsMoodflix\";
            string nombreArchivo = "Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
            string rutaCompleta = Path.Combine(carpetaFisica, nombreArchivo);

            int resultado = bllBackupService.CreateBackup(rutaCompleta);

            if (File.Exists(rutaCompleta))
            {
                // ===============================================================
                // REGISTRAR BITÁCORA — SIN RECÁLCULO DE DV
                // ===============================================================
                var usuario = Session["Usuario"] as BE.Usuario;

                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.Backup,
                        Operacion = TipoOperacion.GenerarCopia
                    };

                    bllBitacora.Insertar(bitacora);
                }

                // ===============================================================
                // ENVÍO DEL ARCHIVO AL NAVEGADOR (DESCARGA)
                // ===============================================================
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombreArchivo);
                Response.TransmitFile(rutaCompleta);

                Response.End();
            }
            else
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "BackupError",
                    "alert('No se encontró el archivo del backup generado.');",
                    true
                );
            }
        }

        // ===================================================
        // RESTORE
        // ===================================================
        protected void btnComenzarRestore_OnClick(object sender, EventArgs e)
        {
            if (!FileUploadRestore.HasFile)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert3",
                    "alert('Seleccione un archivo .bak para continuar.');",
                    true
                );
                return;
            }

            string carpetaRestore = @"C:\BackupsMoodflix\";
            string nombreArchivo = FileUploadRestore.FileName;
            string rutaCompleta = Path.Combine(carpetaRestore, nombreArchivo);

            txtRutaRestore.Text = nombreArchivo;

            try
            {
                FileUploadRestore.SaveAs(rutaCompleta);

                // =============================
                // EJECUTAR RESTORE REAL
                // =============================
                bllBackupService.RestoresBackup(rutaCompleta);

                // =============================
                // REGISTRAR BITÁCORA SIN RECÁLCULO
                // =============================
                var usuario = Session["Usuario"] as BE.Usuario;

                if (usuario != null)
                {
                    Services.Bitacora bitacora = new Services.Bitacora()
                    {
                        Fecha = DateTime.Now,
                        User = usuario,
                        Modulo = TipoModulo.Backup,
                        Operacion = TipoOperacion.Restore,
                    };

                    bllBitacora.Insertar(bitacora);
                }

                txtRutaRestore.Text = "";
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert1",
                    "alert('Restore completado con éxito.');",
                    true
                );
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert2",
                    $"alert('Error durante el restore: {ex.Message}');",
                    true
                );
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            GestorIdioma.Instancia.Desuscribir(this);
        }
    }
}
