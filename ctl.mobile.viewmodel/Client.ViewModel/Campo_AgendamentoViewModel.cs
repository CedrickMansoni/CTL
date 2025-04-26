using System;
using System.Text.Json;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Campo;

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
}
