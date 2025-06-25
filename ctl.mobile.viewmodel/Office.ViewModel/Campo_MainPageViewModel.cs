using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Campo;

namespace ctl.mobile.viewmodel.Office.ViewModel;

public class Campo_MainPageViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;
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

            foreach (var item in a)
            {
                var u = Campos.FirstOrDefault(x => x.Id == item.Id);
                if (u is null) continue;
                if (u.Nome != item.Nome || u.Preco != item.Preco)
                {
                    int index = Campos.IndexOf(u);
                    Campos.RemoveAt(index);
                    Campos.Insert(index, item);
                }
            }

            // Adiciona apenas os que ainda não estão em Campos
            var novosCampos = a.Except(Campos, new ListarCampoDtoComparer()).ToList();
            foreach (var campo in novosCampos)
            {
                Campos.Add(campo);
            }
        }
    });

    public ICommand DeletarCampoCommand => new Command<Listar_Campo_DTO>(async c =>
    {
        var resposta = await Shell.Current.DisplayAlert("Alerta", $"Deseja remover o campo nº {c.Nome}", "Sim", "Não");
        if (!resposta) return;

        var response = await client.DeleteAsync($"deletar/campo/{c.Id}");
        if (response.IsSuccessStatusCode)
        {
            var successMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Sucesso", $"{successMessage}", "Ok");

            int index = Campos.IndexOf(c);
            Campos.RemoveAt(index);
            return;
        }
        var errorMessage = await response.Content.ReadAsStringAsync();
        await Shell.Current.DisplayAlert("Erro", $"{errorMessage}", "Ok");
    });

    public ICommand GotoAddCampoCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Campo_OfficePageAdd");
    });

    public ICommand GotoEditeCampoCommand => new Command<Listar_Campo_DTO>(async c =>
    {
        var campoEdite = new Editar_Campo_DTO()
        {
            Id = c.Id,
            Nome = c.Nome,
            Preco = c.Preco
        };
        var m = JsonSerializer.Serialize<Editar_Campo_DTO>(campoEdite);
        await Shell.Current.GoToAsync($"Campo_OfficePageEdite?campo={Uri.EscapeDataString(m)}");
    });

    public ICommand GotoAgendamentoCommand => new Command<Listar_Campo_DTO>(async campo =>
    {
        string campoJson = JsonSerializer.Serialize(campo);
        await Shell.Current.GoToAsync($"Campo_AgendamentoPage?campoSelecionado={Uri.EscapeDataString(campoJson)}");
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
