using System;

namespace BE
{
    public class UsuarioEstado
    {
        public int IdUsuario { get; set; }

        /// <summary>
        /// Cantidad de intentos fallidos de acceso.
        /// </summary>
        public byte Intentos { get; set; }

        /// <summary>
        /// Indica si el usuario está bloqueado.
        /// true = bloqueado, false = activo
        /// </summary>
        public bool Bloqueado { get; set; }

        // Propiedad de conveniencia para la UI
        public string EstadoTexto
        {
            get { return Bloqueado ? "Bloqueado" : "Activo"; }
        }
    }
}
