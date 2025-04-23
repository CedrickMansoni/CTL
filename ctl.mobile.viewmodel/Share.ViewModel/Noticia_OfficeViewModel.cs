using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Noticia;

namespace ctl.mobile.viewmodel.Share.ViewModel;

public class Noticia_OfficeViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Noticia_OfficeViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private Noticia_DTO noticia = new();
    public Noticia_DTO Noticia
    {
        get { return noticia; }
        set
        {
            noticia = value;
            OnPropertyChanged(nameof(Noticia));
        }
    }

    private ObservableCollection<Noticia_DTO> noticias = [];
    public ObservableCollection<Noticia_DTO> Noticias
    {
        get { return noticias; }
        set
        {
            noticias = value;
            OnPropertyChanged(nameof(Noticias));
        }
    }


    public ICommand ListarNoticiaCommand => new Command(async () =>
    {
        await Shell.Current.DisplayAlert("E0", "Ainda não entrou", "Ok");
        var response = await client.GetAsync("listar/noticia");
        await Shell.Current.DisplayAlert("E1", "Fez a requisição", "Ok");
        if (response.IsSuccessStatusCode)
        {
            await Shell.Current.DisplayAlert("E3", "Serializou", "Ok");
            using var json = await response.Content.ReadAsStreamAsync();
            Noticias = JsonSerializer.Deserialize<ObservableCollection<Noticia_DTO>>(json, options) ?? [];
        }
    });

    private bool activity = false;
    public bool Activity
    {
        get => activity;
        set
        {
            activity = value;
            OnPropertyChanged(nameof(Activity));
        }
    }

    private bool enablePage = true;
    public bool EnablePage
    {
        get => enablePage;
        set
        {
            enablePage = value;
            OnPropertyChanged(nameof(EnablePage));
        }
    }

    public ICommand ActivityCommand => new Command(() =>
    {
        EnablePage = !EnablePage;
        Activity = !Activity;
    });


}
