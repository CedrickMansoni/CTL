using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Noticia;

namespace ctl.mobile.viewmodel.Office.ViewModel;

public class Home_OfficeViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Home_OfficeViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        ListarNoticiasCommand.Execute(null);
    }

    private Noticia_DTO noticia = new();
    public Noticia_DTO Noticia
    {
        get => noticia;
        set
        {
            noticia = value;
            OnPropertyChanged(nameof(Noticia));
        }
    }

    private ObservableCollection<Noticia_DTO>? noticias = [];
    public ObservableCollection<Noticia_DTO>? Noticias
    {
        get => noticias;
        set
        {
            noticias = value;
            OnPropertyChanged(nameof(Noticias));
        }
    }

    public ICommand ListarNoticiasCommand => new Command(async () =>
    {
        var response = await client.GetAsync("listar/noticia");
        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            Noticias = JsonSerializer.Deserialize<ObservableCollection<Noticia_DTO>>(stream, options);
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Erro", error, "OK");
        }
    });

    public ICommand AdicionarnoticiaCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Noticia_OfficePage");
    });

}
