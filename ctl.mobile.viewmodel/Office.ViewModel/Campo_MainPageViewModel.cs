using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Campo;

namespace ctl.mobile.viewmodel.Office.ViewModel;

public class Campo_MainPageViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Campo_MainPageViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private ObservableCollection<Listar_Campo_DTO> campos = new();
    public ObservableCollection<Listar_Campo_DTO> Campos
    {
        get => campos;
        set
        {
            campos = value;
            OnPropertyChanged(nameof(Campos));
        }
    }

    public ICommand ListarCampoCommand => new Command(async () =>
    {
        var response = await client.GetAsync("listar/campo");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Campos = JsonSerializer.Deserialize<ObservableCollection<Listar_Campo_DTO>>(json, options) ?? [];
        }
    });

    public ICommand GotoAddCampoCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Campo_OfficePageAdd");
    });
    
}
     