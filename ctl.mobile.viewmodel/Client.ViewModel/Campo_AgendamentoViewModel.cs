using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Campo;
using ctl.share.DTO_App.Marcacao;
using Microsoft.Maui.Platform;

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

        ListarMarcacoesCommand.Execute(null);
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

    private DateTime dataMarcacao = DateTime.Now.Date;
    public DateTime DataMarcacao
    {
        get => dataMarcacao;
        set
        {
            dataMarcacao = value;
            OnPropertyChanged(nameof(DataMarcacao));
            Marcacao.Clear();
            Shell.Current.DisplayAlert("Erro", "Mais e mais bugs", "Ok");
            ListarMarcacoesCommand.Execute(null);
        }
    }

    private TimeSpan horaInicio = new();
    public TimeSpan HoraInicio
    {
        get => horaInicio;
        set
        {
            horaInicio = value;
            OnPropertyChanged(nameof(HoraInicio));
        }
    }

    private TimeSpan horaTermino = new();
    public TimeSpan HoraTermino
    {
        get => horaTermino;
        set
        {
            horaTermino = value;
            OnPropertyChanged(nameof(HoraTermino));
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

    public ICommand ListarMarcacoesCommand => new Command(async () =>
    {  
        int p = Marcacao.Count == 0 ? 0 : 1;
        var response = await client.GetAsync($"listar/marcacoes?idCampo={Campo.Id}&dataMarcacao={DataMarcacao}&skip={Marcacao.Count + 30 * p}&take={30}");
        if (!response.IsSuccessStatusCode) return;
        using var m = await response.Content.ReadAsStreamAsync();
        var data = JsonSerializer.Deserialize<ObservableCollection<Listar_Marcacao_DTO>>(m, options) ?? [];

        if (data == null) return;

        if (Marcacao.Count == 0)
        {
            Marcacao = data;
            return;
        }

        // Adiciona apenas os que ainda não estão em Campos
        var novasNoticias = data.Except(Marcacao, new ListarMarcacaoDtoComparer()).ToList();
        foreach (var n in novasNoticias)
        {
            Marcacao.Insert(0, n);
        }
    
    });

    public ICommand FazerMarcacaoCommand => new Command(async () =>
    {        
        if(HoraInicio.Hours <= 5 || HoraTermino.Hours <= 5)
        {
            await Shell.Current.DisplayAlert("Erro", "O Clube de ténis não trabalha antes das 5h00", "Ok");
            return;
        }

        if (HoraInicio.Hours >= 22 || HoraTermino.Hours >= 22)
        {
            await Shell.Current.DisplayAlert("Erro", "O Clube de ténis não trabalha depois das 22h00", "Ok");
            return;
        }

        if (HoraInicio.Hours >= HoraTermino.Hours)
        {
            await Shell.Current.DisplayAlert("Erro", "A hora de início não pode ser maior ou igual a hora de término", "Ok");
            return;
        }
        
        var marcacaoNova = new Fazer_Marcacao_DTO
        {
            IdCampo = Campo.Id,
            IdCliente = Convert.ToInt32(await SecureStorage.Default.GetAsync("usuarioId")),
            DataMarcacao = DataMarcacao,
            DataInicio = DataMarcacao.AddHours(HoraInicio.Hours).AddMinutes(HoraInicio.Minutes),
            DataTermino = DataMarcacao.AddHours(HoraTermino.Hours).AddMinutes(HoraTermino.Minutes)

        };
        string json = JsonSerializer.Serialize<Fazer_Marcacao_DTO>(marcacaoNova, options);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("fazer/marcacao", content);
        if (response.IsSuccessStatusCode)
        {
            await Shell.Current.DisplayAlert("Sucesso", $"Solicitação de agendamento realizada com sucesso", "Ok");
            ListarMarcacoesCommand.Execute(null);
            return;
        }
        await Shell.Current.DisplayAlert("Erro", "Solicitação de agendamento não foi realizada com sucesso", "Ok");
        return;
    });
}

public class ListarMarcacaoDtoComparer : IEqualityComparer<Listar_Marcacao_DTO>
{
    public bool Equals(Listar_Marcacao_DTO? x, Listar_Marcacao_DTO? y)
    {
        if (x is null || y is null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Listar_Marcacao_DTO obj)
    {
        return obj.Id.GetHashCode();
    }
}
