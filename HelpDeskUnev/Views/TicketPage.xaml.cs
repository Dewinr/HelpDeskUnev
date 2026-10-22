#if ANDROID || IOS || MACCATALYST || WINDOWS
using HelpDeskUnev.Models;
using HelpDeskUnev.Services;

namespace HelpDeskUnev.Views
{
    public partial class TicketPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public TicketPage(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarDatos();
        }

        private async Task CargarDatos()
        {
            var clientes = await _databaseService.GetClientesAsync();
            var productos = await _databaseService.GetProductosAsync();

            pickerClientes.ItemsSource = clientes;
            pickerProductos.ItemsSource = productos;
        }

        [Obsolete]
        private async void OnGuardarTicketClicked(object sender, EventArgs e)
        {
            var clienteSeleccionado = pickerClientes.SelectedItem as Cliente;
            var productoSeleccionado = pickerProductos.SelectedItem as Producto;

            if (clienteSeleccionado == null)
            {
                await DisplayAlert("Error", "Debe seleccionar un cliente.", "OK");
                return;
            }

            if (productoSeleccionado == null)
            {
                await DisplayAlert("Error", "Debe seleccionar un producto.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAsunto.Text))
            {
                await DisplayAlert("Error", "Ingrese el asunto del ticket.", "OK");
                return;
            }

            var ticket = new Models.Ticket
            {
                ClienteId = clienteSeleccionado.Id,
                ClienteNombre = $"{clienteSeleccionado.Nombre} {clienteSeleccionado.Apellido}",
                ProductoId = productoSeleccionado.Id,
                ProductoNombre = productoSeleccionado.Nombre,
                Asunto = txtAsunto.Text,
                Descripcion = txtDescripcion.Text ?? string.Empty,
                Prioridad = pickerPrioridad.SelectedItem?.ToString() ?? "Media",
                Estado = "Abierto",
                FechaCreacion = DateTime.Now
            };

            await _databaseService.SaveTicketAsync(ticket);
            await DisplayAlert("Ticket Creado",
                               $"Ticket generado con éxito para {ticket.ClienteNombre}.",
                               "OK");

            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            pickerClientes.SelectedIndex = -1;
            pickerProductos.SelectedIndex = -1;
            pickerPrioridad.SelectedIndex = -1;
            txtAsunto.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }
    }
#endif