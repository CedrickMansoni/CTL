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

    private ObservableCollection<Listar_Campo_DTO> campos = [];
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
            using var json = await response.Content.ReadAsStreamAsync();

            var a = JsonSerializer.Deserialize<ObservableCollection<Listar_Campo_DTO>>(json, options);
            if (a == null) return;

            if (Campos.Count == 0)
            {
                Campos = a;
                return;
            }

            // Adiciona apenas os que ainda não estão em Campos
            var novosCampos = a.Except(Campos, new ListarCampoDtoComparer()).ToList();
            foreach (var campo in novosCampos)
            {
                Campos.Add(campo);
            }
        }
    });

    public ICommand GotoAddCampoCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Campo_OfficePageAdd");
    });

}


public class ListarCampoDtoComparer : IEqualityComparer<Listar_Campo_DTO>
{
    public bool Equals(Listar_Campo_DTO? x, Listar_Campo_DTO? y)
    {
        if (x is null || y is null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Listar_Campo_DTO obj)
    {
        return obj.Id.GetHashCode();
    }
}
