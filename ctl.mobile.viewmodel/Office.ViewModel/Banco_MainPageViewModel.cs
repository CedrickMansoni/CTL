using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Banco;

namespace ctl.mobile.viewmodel.Office.ViewModel;

public class Banco_MainPageViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Banco_MainPageViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private ObservableCollection<Banco_Response_DTO> bancos = [];
    public ObservableCollection<Banco_Response_DTO> Bancos
    {
        get => bancos;
        set
        {
            bancos = value;
            OnPropertyChanged(nameof(Bancos));
        }
    }

    public ICommand ListarBancoCommand => new Command(async () =>
    {
        var response = await client.GetAsync("listar/bancos");
        if (response.IsSuccessStatusCode)
        {
            using var json = await response.Content.ReadAsStreamAsync();

            var a = JsonSerializer.Deserialize<ObservableCollection<Banco_Response_DTO>>(json, options);
            if (a == null) return;

            if (Bancos.Count == 0)
            {
                Bancos = a;
                return;
            }

            // Adiciona apenas os que ainda não estão em Campos
            var novoBanco = a.Except(Bancos, new ListarBancoDtoComparer()).ToList();
            foreach (var n in novoBanco)
            {
                Bancos.Insert(0, n);
            }
        }
    });

    public ICommand CopiarContaOrIbanCommand => new Command<Banco_Response_DTO>(async (dados) =>
    {
        var resposta = await Shell.Current.DisplayAlert("Área de transferência", "O que deseja copiar para a área de transferência?", "CONTA", "IBAN");
        
        if (resposta)
        {
            ContaCopyToClipboardCommand.Execute(dados.Conta);
        }
        else
        {
            IbanCopyToClipboardCommand.Execute(dados.IBAN);
        }
    });

    public ICommand ContaCopyToClipboardCommand => new Command<string>(async (p) =>
    {
        if (string.IsNullOrEmpty(p)) return;

        await Clipboard.SetTextAsync(p);
        await Shell.Current.DisplayAlert("Conta", "Conta bancária copiada para a área de transferência", "OK");
    });

        public ICommand IbanCopyToClipboardCommand => new Command<string>(async (p) =>
    {
        if (string.IsNullOrEmpty(p)) return;

        await Clipboard.SetTextAsync(p);
        await Shell.Current.DisplayAlert("IBAN", "IBAN copiado para a área de transferência", "OK");
    });

    public ICommand GotoAddBancoPageCommand => new Command(async () =>
    {
        ActivityCommand.Execute(null);
        await Shell.Current.GoToAsync("Banco_OfficePageAdd");
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


public class ListarBancoDtoComparer : IEqualityComparer<Banco_Response_DTO>
{
    public bool Equals(Banco_Response_DTO? x, Banco_Response_DTO? y)
    {
        if (x is null || y is null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Banco_Response_DTO obj)
    {
        return obj.Id.GetHashCode();
    }
}
