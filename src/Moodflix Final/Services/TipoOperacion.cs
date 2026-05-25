using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public enum TipoOperacion
    {
        Login,
        Logout,
        Desconocida,
        CambioIdioma,
        Filtrar,
        GenerarCopia,
        Restore,
        CrearFamilia,
        AgregarRol,
        QuitarRol,
        AsignarFamilia,
        QuitarFamilia,
        AltaPelicula,
        ModificarPelicula,
        BorrarPelicula,
        AltaLibro,
        ModificarLibro,
        BorrarLibro,
        Bloquear,
        Desbloquear,
        AgregarCarrito,
        EliminarFamilia,
        RecomponerDV,
    }
}
