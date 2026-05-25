using DAL;
using BE;
using System.Collections.Generic;

namespace BLL
{
    public class UsuarioEstadoBLL
    {
        private readonly MP_UsuarioEstado _mp = new MP_UsuarioEstado();

        // Obtener registro por usuario
        public UsuarioEstadoDTO Obtener(int idUsuario)
        {
            return _mp.GetById(idUsuario);
        }

        // Reinicia intentos y desbloquea
        public void ReiniciarIntentos(int idUsuario)
        {
            var estado = Obtener(idUsuario);
            if (estado == null) return;

            estado.Intentos = 0;
            estado.Bloqueado = false;

            _mp.Update(estado);
        }

        // Suma 1 intento fallido y si llega a 3 bloquea
        public void RegistrarIntentoFallido(int idUsuario)
        {
            var estado = Obtener(idUsuario);
            if (estado == null) return;

            estado.Intentos++;

            if (estado.Intentos >= 3)
                estado.Bloqueado = true;

            _mp.Update(estado);
        }

        // Lista estados de usuario (para la grilla)
        public List<UsuarioEstadoDTO> ListarUsuariosEstado()
        {
            return _mp.GetAll();
        }


        // Bloqueo manual desde la UI
        public void Bloquear(int idUsuario)
        {
            var estado = Obtener(idUsuario);
            if (estado == null) return;

            estado.Bloqueado = true;

            _mp.Update(estado);
        }

        // Desbloqueo manual desde la UI
        public void Desbloquear(int idUsuario)
        {
            var estado = Obtener(idUsuario);
            if (estado == null) return;

            estado.Bloqueado = false;
            estado.Intentos = 0; // al desbloquear, reset a 0

            _mp.Update(estado);
        }
    }
}
