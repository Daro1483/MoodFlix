public class UsuarioEstadoDTO
{
    public int IdUsuario { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public int Intentos { get; set; }
    public bool Bloqueado { get; set; }
}
