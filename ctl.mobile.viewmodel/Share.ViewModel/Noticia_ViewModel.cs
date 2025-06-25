using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Noticia;

namespace ctl.mobile.viewmodel.Share.ViewModel;

public class Noticia_ViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;
    public Noticia_ViewModel()
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

    private ObservableCollection<Noticia_DTO> noticias = [];
    public ObservableCollection<Noticia_DTO> Noticias
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
            if (response.IsSuccessStatusCode)
            {
                using var json = await response.Content.ReadAsStreamAsync();

                var a = JsonSerializer.Deserialize<ObservableCollection<Noticia_DTO>>(json, options);
                if (a == null) return;

                if (Noticias.Count == 0)
                {
                    Noticias = a;
                    return;
                }

                foreach (var item in a)
                {
                    var u = Noticias.FirstOrDefault(x => x.Id == item.Id);
                    if (u is null) continue;
                    
                    if (u.Titulo != item.Titulo || u.Materia != item.Materia || u.Imagem != item.Imagem)
                    {
                        int index = Noticias.IndexOf(u);
                        Noticias.RemoveAt(index);
                        Noticias.Insert(index, item);
                    }
                }

                // Adiciona apenas os que ainda não estão em Campos
                var novasNoticias = a.Except(Noticias, new ListarNoticiaDtoComparer()).ToList();
                foreach (var n in novasNoticias)
                {
                    Noticias.Insert(0, n);
                }
            }
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Erro", error, "OK");
        }
    });

    public ICommand AdicionarnoticiaCommand => new Command(async () =>
    {
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync("Noticia_OfficePage");
        ActivityCommand.Execute(null);
    });

    public ICommand VerDetalhesCommand => new Command<Noticia_DTO>(async n =>
    {
        var noticiaJSON = JsonSerializer.Serialize(n);
        await Shell.Current.GoToAsync($"NoticiaDetalhePage?noticia={Uri.EscapeDataString(noticiaJSON)}", true);
    });

    public ICommand DeletarNoticiaCommand => new Command<Noticia_DTO>(async n =>
    {
        var resposta = await Shell.Current.DisplayAlert("Alerta", "Deseja apagar a notícia?", "Sim", "Não");
        if (!resposta) return;

        var response = await client.DeleteAsync($"deletar/noticia/{n.Id}");
        if (response.IsSuccessStatusCode)
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Sucesso", $"{successMessage}", "Ok");
            int index = Noticias.IndexOf(n);
            Noticias.RemoveAt(index);
            return;
        }
        var errorMessage = await response.Content.ReadAsStringAsync();
        await Shell.Current.DisplayAlert("Erro", $"{errorMessage}", "Ok");

    });

    public ICommand EditarNoticiaCommand => new Command<Noticia_DTO>(async n =>
{
    ActivityCommand.Execute(null);
    var m = JsonSerializer.Serialize<Noticia_DTO>(n);
    await Shell.Current.GoToAsync($"NoticiaEdite_OfficePage?noticia={Uri.EscapeDataString(m)}");
    ActivityCommand.Execute(null);
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


public class ListarNoticiaDtoComparer : IEqualityComparer<Noticia_DTO>
{
    public bool Equals(Noticia_DTO? x, Noticia_DTO? y)
    {
        if (x is null || y is null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Noticia_DTO obj)
    {
        return obj.Id.GetHashCode();
    }
}