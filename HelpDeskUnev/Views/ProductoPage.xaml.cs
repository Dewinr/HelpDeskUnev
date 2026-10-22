#if ANDROID || IOS || MACCATALYST || WINDOWS
using HelpDeskUnev.Models;
using HelpDeskUnev.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskUnev.Views
{
    public partial class ProductoPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public ProductoPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }

        private async void OnGuardarProductoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreProd.Text))
            {
                await DisplayAlert("Error", "El nombre del producto es obligatorio.", "OK");
                return;
            }

            decimal.TryParse(txtPrecio.Text, out decimal precio);
            int.TryParse(txtStock.Text, out int stock);

            var producto = new Producto
            {
                Nombre = txtNombreProd.Text,
                Categoria = txtCategoria.Text ?? string.Empty,
                Precio = precio,
                Stock = stock,
                Descripcion = txtDescripcion.Text ?? string.Empty
            };

            await _databaseService.SaveProductoAsync(producto);
            await DisplayAlert("Éxito", "Producto guardado correctamente en la base de datos.", "OK");

            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtNombreProd.Text = string.Empty;
            txtCategoria.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtStock.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }
    }
}
#endif