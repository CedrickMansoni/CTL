using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Usuario;

namespace ctl.mobile.viewmodel.Client.ViewModel;

public class Conta_ClientPage_ViewmModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public Conta_ClientPage_ViewmModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private Cadastrar_Usuario_DTO usuario = new();
    public Cadastrar_Usuario_DTO Usuario
    {
        get => usuario;
        set
        {
            usuario = value;
            OnPropertyChanged(nameof(Usuario));
        }
    }

    public ICommand CadastrarCommand => new Command(async () =>
    {
        if (string.IsNullOrEmpty(Usuario.Nome))
        {
            await Shell.Current.DisplayAlert("Erro", "Nome não pode ser vazio", "OK");
            return;
        }
        if (string.IsNullOrEmpty(Usuario.Telefone))
        {
            await Shell.Current.DisplayAlert("Erro", "Telefone não pode ser vazio", "OK");
            return;
        }
        if (string.IsNullOrEmpty(Usuario.Senha))
        {
            await Shell.Current.DisplayAlert("Erro", "Senha não pode ser vazio", "OK");
            return;
        }

        ActivityCommand.Execute(null);

        Usuario.IdTipo = 1;

        var json = JsonSerializer.Serialize(Usuario, options);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("cadastrar/cliente", content);

        if (response.IsSuccessStatusCode)
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Sucesso", $"{Usuario.Nome} Cadastro realizado com sucesso", "OK");
            await Shell.Current.GoToAsync("..");
            // Handle success
        }
        else
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Erro", "Não conseguimos criar a sua conta, por favorcontacte o suporte técnico", "OK");
            // Handle error
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
