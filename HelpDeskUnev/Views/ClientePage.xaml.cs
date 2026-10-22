#if ANDROID || IOS || MACCATALYST || WINDOWS

using HelpDeskUnev.Models;
using HelpDeskUnev.Services;

namespace HelpDeskUnev.Views;

public partial class ClientePage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public ClientePage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
    }

    private async void OnGuardarClienteClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
        {
            await DisplayAlert("Error", "Por favor, complete los campos obligatorios.", "OK");
            return;
        }

        var cliente = new Cliente
        {
            Nombre = txtNombre.Text,
            Apellido = txtApellido.Text,
            Telefono = txtTelefono.Text ?? string.Empty,
            Email = txtEmail.Text ?? string.Empty
        };

        await _databaseService.SaveClienteAsync(cliente);
        await DisplayAlert("Éxito", "Cliente guardado correctamente.", "OK");

        LimpiarFormulario();
    }

    private void LimpiarFormulario()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtTelefono.Text = string.Empty;
        txtEmail.Text = string.Empty;
    }
#endif
