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
    public partial class Registrarse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LinkLogin_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        BLL.Usuario bllUsuario = new BLL.Usuario();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();
        protected void btnRegistrarse_OnClick(object sender, EventArgs e)
        {

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string rePassword = TextBox1.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword))
            {
                MostrarError("Todos los campos son obligatorios.");
                return;
            }

            if (password != rePassword)
            {
                MostrarError("Las contraseñas no coinciden.");
                return;
            }

            
                
            Usuario usuario = new Usuario
            {
                Username = username,
                Email = email,
                Password = CryptoManager.Hash(password),
            };

            if (bllUsuario.Insertar(usuario) > 0)
            {
                bllDvh.Recalcular(bllDvh.Listar(), bllUsuario.Listar());
                bllDvv.Recalcular();

                ClientScript.RegisterStartupScript(this.GetType(), "RegistroExitoso", "alert('Se registró correctamente el usuario');", true);
                //Response.Redirect("Login.aspx");
                
            }
            else
            {
                MostrarError("Ocurrio un error al registrar el usuario");
                return;
            }


        }

        private void MostrarError(string mensaje)
        {
            pnlError.Visible = true;
            lblError.Text = mensaje;
        }



    }
}