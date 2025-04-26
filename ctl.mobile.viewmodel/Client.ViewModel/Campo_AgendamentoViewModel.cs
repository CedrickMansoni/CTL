using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Campo;
using ctl.share.DTO_App.Marcacao;

namespace ctl.mobile.viewmodel.Client.ViewModel;

[QueryProperty(nameof(CampoSelecionado), "campoSelecionado")]
public class Campo_AgendamentoViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;

    public Campo_AgendamentoViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }

    private string campoSelecionado = string.Empty;
    public string CampoSelecionado
    {
        get => campoSelecionado;
        set
        {
            campoSelecionado = value;
            if (string.IsNullOrEmpty(CampoSelecionado)) return;
            Campo = JsonSerializer.Deserialize<Listar_Campo_DTO>(CampoSelecionado) ?? new Listar_Campo_DTO();
            OnPropertyChanged(nameof(CampoSelecionado));
        }
    }

    private Listar_Campo_DTO campo = new();
    public Listar_Campo_DTO Campo
    {
        get => campo;
        set
        {
            campo = value;

            OnPropertyChanged(nameof(Campo));
        }
    }

    private ObservableCollection<Listar_Marcacao_DTO> marcacao = [];
    public ObservableCollection<Listar_Marcacao_DTO> Marcacao
    {
        get => marcacao;
        set
        {
            marcacao = value;
            OnPropertyChanged(nameof(Marcacao));
        }
    }

    public ICommand ListarMarcacoesCommand => new Command<int>(async idCampo =>
    {
        int p = Marcacao.Count == 0 ? 0 : 1;
        var response = await client.GetAsync($"listar/marcacoes?idCampo={idCampo}&skip={Marcacao.Count + 30 * p}&take={30}");
        if (!response.IsSuccessStatusCode) return;
        using var m = await response.Content.ReadAsStreamAsync();
        Marcacao = JsonSerializer.Deserialize<ObservableCollection<Listar_Marcacao_DTO>>(m, options) ?? [];
    });
}
