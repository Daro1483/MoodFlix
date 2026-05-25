using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BE;
using Services;

namespace Moodflix
{
    public partial class Libros : System.Web.UI.Page, IIdiomaObserver
    {
        BLL.Libro bllLibro = new BLL.Libro();
        BLL.Bitacora bllBitacora = new BLL.Bitacora();
        BLL.DVH bllDvh = new BLL.DVH();
        BLL.DVV bllDvv = new BLL.DVV();
        private string emocion = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Emotion value (siempre asignar primero el valor)
            emocion = Session["Emocion"] as string;
            if (string.IsNullOrEmpty(emocion))
            {
                emocion = TipoEmocion.Aburrido.ToString();
            }
            lblEmocion.InnerHtml = emocion; // lblEmocion muestra solo el valor de la emoción

            // 2) Cargar lista de libros si hace falta (solo la primera vez)
            if (!IsPostBack)
            {
                // Evitar null: si bllLibro.Listar() devuelve null, guardamos una lista vacía
                var lista = bllLibro.Listar() ?? new List<Libro>();
                Session["Libros"] = lista;
            }

            // 3) Suscribirse al GestorIdioma correcto (usar la instancia singleton)
            try
            {
                var gestor = GestorIdioma.Instancia; // usar la misma instancia que en Site.Master
                if (gestor != null)
                {
                    gestor.Suscribir(this); // nos suscribimos siempre (ok hacerlo en cada Page_Load)
                    // Aplicar idioma actual inmediatamente (para que el txtEmocion se vea al entrar)
                    var idiomaActual = gestor.ObtenerIdiomaActual();
                    if (idiomaActual != null)
                        ActualizarIdioma(idiomaActual);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al suscribir al GestorIdioma en Libros: " + ex.Message);
            }

            // 4) Generar tarjetas (asegurarse de limpiar el contenedor antes)
            GenerateCards();
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma == null || idioma.Traducciones == null) return;

            // Actualizar la etiqueta traducible que dice "Emoción:" -> txtEmocion
            if (idioma.Traducciones.TryGetValue("Libros_Emocion", out string textoEmocion))
                txtEmocion.InnerHtml = textoEmocion + ": ";
            else
                txtEmocion.InnerHtml = "Emoción: "; // fallback

            // Actualizar los textos dinámicos de las tarjetas (botones y headers)
            // Recorremos cardsContainer y actualizamos sus controles
            foreach (Control c in cardsContainer.Controls)
            {
                ActualizarControlesInternos(c, idioma);
            }
        }

        private void ActualizarControlesInternos(Control contenedor, Idioma idioma)
        {
            foreach (Control c in contenedor.Controls)
            {
                // card-header (HtmlGenericControl)
                if (c is HtmlGenericControl div && div.Attributes["class"] != null && div.Attributes["class"].Contains("card-header"))
                {
                    if (idioma.Traducciones.TryGetValue("Libros_Libro", out string textoLibro))
                        div.InnerText = textoLibro;
                    else
                        div.InnerText = "Libro";
                }
                // Botones
                else if (c is Button btn)
                {
                    if (idioma.Traducciones.TryGetValue("Libros_BotonAgregar", out string textoAgregar))
                        btn.Text = textoAgregar;
                    else if (idioma.Traducciones.TryGetValue("Libros_Agregar", out string textoAgregarAlt))
                        btn.Text = textoAgregarAlt;
                    else
                        btn.Text = "Agregar";
                }
                else
                {
                    // Recursividad
                    if (c.HasControls())
                        ActualizarControlesInternos(c, idioma);
                }
            }
        }

        private void GenerateCards()
        {
            // Limpiar el contenedor para evitar duplicados
            cardsContainer.Controls.Clear();

            List<Libro> libros = Session["Libros"] as List<Libro> ?? new List<Libro>();

            // Si la lista está vacía, podés mostrar un mensaje (debug)
            if (libros.Count == 0)
            {
                // opcional: mostrar un label con "No hay libros"
                System.Diagnostics.Debug.WriteLine("No hay libros en Session[\"Libros\"].");
            }

            var librosFiltrados = libros.Where(l => l.Emocion != null && l.Emocion.TipoEmocion.ToString() == emocion).ToList();

            foreach (var libro in librosFiltrados)
            {
                string title = libro.Nombre;
                string imageUrl = libro.Uri;
                float price = libro.Precio;

                HtmlGenericControl divCol = new HtmlGenericControl("div");
                divCol.Attributes.Add("class", "col-6 col-sm-4 col-md-3");

                HtmlGenericControl divCard = new HtmlGenericControl("div");
                divCard.Attributes.Add("class", "card card-shadow mb-2");

                HtmlGenericControl divCardHeader = new HtmlGenericControl("div");
                divCardHeader.Attributes.Add("class", "card-header");
                // poner texto por defecto; se traducirá luego por ActualizarIdioma si se suscribe correctamente
                divCardHeader.InnerText = "Libro";

                HtmlGenericControl imgCard = new HtmlGenericControl("img");
                imgCard.Attributes.Add("class", "card-img-top");
                imgCard.Attributes.Add("src", imageUrl);

                HtmlGenericControl divCardBody = new HtmlGenericControl("div");
                divCardBody.Attributes.Add("class", "card-body");

                HtmlGenericControl divFlex = new HtmlGenericControl("div");
                divFlex.Attributes.Add("class", "d-flex flex-column flex-md-row justify-content-between align-items-center");

                HtmlGenericControl divMarginBodyTittle = new HtmlGenericControl("div");
                divMarginBodyTittle.Attributes.Add("class", "mb-2 mb-md-0");

                HtmlGenericControl h6CardTitle = new HtmlGenericControl("h6");
                h6CardTitle.Attributes.Add("class", "card-title");
                h6CardTitle.InnerText = title;

                HtmlGenericControl divCardText = new HtmlGenericControl("div");
                divCardText.Attributes.Add("class", "card-text");
                divCardText.InnerText = price.ToString("C");

                Button btnAgregarCarrito = new Button();
                btnAgregarCarrito.CssClass = "btn btn-primary btn-sm";
                btnAgregarCarrito.CommandArgument = libro.ID.ToString();
                btnAgregarCarrito.Click += BtnAgregarCarrito_Click;
                btnAgregarCarrito.Text = "Agregar"; // se actualizará con ActualizarIdioma

                divMarginBodyTittle.Controls.Add(h6CardTitle);
                divMarginBodyTittle.Controls.Add(divCardText);

                divFlex.Controls.Add(divMarginBodyTittle);
                divFlex.Controls.Add(btnAgregarCarrito);

                divCardBody.Controls.Add(divFlex);
                divCard.Controls.Add(divCardHeader);
                divCard.Controls.Add(imgCard);
                divCard.Controls.Add(divCardBody);
                divCol.Controls.Add(divCard);

                cardsContainer.Controls.Add(divCol);
            }

            // Tras crear las cards, intentar aplicar traducción actual si existe GestorIdioma
            try
            {
                var gestor = GestorIdioma.Instancia;
                var idiomaActual = gestor?.ObtenerIdiomaActual();
                if (idiomaActual != null)
                    ActualizarIdioma(idiomaActual);
            }
            catch { /* swallow */ }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Desuscribir del singleton
            try
            {
                var gestor = GestorIdioma.Instancia;
                if (gestor != null)
                    gestor.Desuscribir(this);
            }
            catch { /* swallow */ }
        }

        private void BtnAgregarCarrito_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int id = int.Parse(btn.CommandArgument);

            List<Producto> carrito = Session["Carrito"] as List<Producto>;
            if (carrito == null)
            {
                carrito = new List<Producto>();
            }

            carrito.Add(bllLibro.ObtenerPorId(id));
            Session["Carrito"] = carrito;

            // 🔹 Registrar en bitácora que se agregó un libro al carrito
            var usuario = Session["Usuario"] as BE.Usuario;

            if (usuario != null)
            {
                Services.Bitacora bitacora = new Services.Bitacora()
                {
                    Fecha = DateTime.Now,
                    User = usuario,
                    Modulo = TipoModulo.Libros,
                    Operacion = TipoOperacion.AgregarCarrito,
                };

                new BLL.Bitacora().Insertar(bitacora);


                bllDvh.Recalcular(bllDvh.Listar(), bllBitacora.Listar());
                bllDvv.Recalcular();
            }



            // ✅ Obtener el texto traducido del GestorIdioma
            string mensaje = "El ítem se agregó al carrito"; // valor por defecto (español)
            try
            {
                var idiomaActual = GestorIdioma.Instancia.ObtenerIdiomaActual();
                if (idiomaActual != null && idiomaActual.Traducciones.TryGetValue("Libros_Alert", out string traduccion))
                {
                    mensaje = traduccion;
                }
            }
            catch { }

            // ✅ Mostrar mensaje traducido
            ClientScript.RegisterStartupScript(this.GetType(), "AgregarCarrito", $"alert('{mensaje}');", true);
        }
    }
}
