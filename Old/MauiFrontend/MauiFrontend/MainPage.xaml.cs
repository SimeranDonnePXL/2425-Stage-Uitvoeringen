using MauiFrontend.Models;
using MauiFrontend.Services;
using System.Collections.ObjectModel;

namespace MauiFrontend;

public partial class MainPage : ContentPage
{
    private readonly PrintService _printerService = new();
    private readonly WebSocketService _ws = new();
    private List<Printer> _printers = new();
    private ObservableCollection<string> _statusFeed = new();

    public MainPage()
    {
        InitializeComponent();
        StatusFeed.ItemsSource = _statusFeed;
        LoadPrinters();
        ConnectWebSocket();
    }

    private async void LoadPrinters()
    {
        _printers = await _printerService.GetPrintersAsync();
        foreach (var printer in _printers)
            PrinterPicker.Items.Add(printer.Name);
    }

    private async void OnPrintClicked(object sender, EventArgs e)
    {
        if (PrinterPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Please select a printer.", "OK");
            return;
        }

        var selectedPrinter = _printers[PrinterPicker.SelectedIndex];
        await _printerService.SubmitPrintJobAsync(selectedPrinter.Id);
    }

    private async void ConnectWebSocket()
    {
        _ws.OnMessage += msg =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _statusFeed.Insert(0, msg);
            });
        };

        await _ws.ConnectAsync();
    }
}