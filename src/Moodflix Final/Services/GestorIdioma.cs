using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace Services
{
    public class GestorIdioma : ISujetoIdioma
    {
        // 🔹 Singleton
        private static GestorIdioma instancia;
        public static GestorIdioma Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new GestorIdioma();
                return instancia;
            }
        }

        // 🔹 Lista de observadores (páginas, controles, etc.)
        private static readonly List<IIdiomaObserver> observadores = new List<IIdiomaObserver>();

        // 🔹 Idioma actual compartido en toda la aplicación
        private static BE.Idioma idiomaActual;

        private GestorIdioma() { }

        // 🔹 Suscripción / Desuscripción
        public void Suscribir(IIdiomaObserver observer)
        {
            if (!observadores.Contains(observer))
                observadores.Add(observer);
        }

        public void Desuscribir(IIdiomaObserver observer)
        {
            if (observadores.Contains(observer))
                observadores.Remove(observer);
        }

        // 🔹 Cambio de idioma
        public void CambiarIdioma(BE.Idioma nuevoIdioma)
        {
            idiomaActual = nuevoIdioma;
            Notificar(); // 🔔 Esto dispara la actualización en todos los observers
        }

        // 🔹 Notificación a todos los observadores
        public void Notificar()
        {           
            foreach (var obs in observadores)
            {
                try
                {                    
                    obs.ActualizarIdioma(idiomaActual);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en Notificar(): " + ex.Message);
                }

            }
        }

        // 🔹 Cargar idioma desde XML
        public BE.Idioma CargarIdioma(string codigoIdioma)
        {
            var idioma = new BE.Idioma
            {
                Codigo = codigoIdioma,
                Traducciones = new Dictionary<string, string>()
            };

            string ruta = HttpContext.Current.Server.MapPath($"~/Resources/Idiomas/{codigoIdioma}.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            foreach (XmlNode nodo in doc.SelectNodes("//Texto"))
            {
                string clave = nodo.Attributes["clave"].Value;
                string valor = nodo.InnerText;
                idioma.Traducciones.Add(clave, valor);
            }

            return idioma;
        }

        // 🔹 Obtener idioma actual (para cuando necesites leerlo)
        public BE.Idioma ObtenerIdiomaActual()
        {
            return idiomaActual;
        }
    }
}