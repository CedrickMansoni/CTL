using System;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Usuario;

namespace ctl.mobile.viewmodel.Share.ViewModel;

public class LoginPage_ViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public LoginPage_ViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private Login_Usuario_DTO usuario = new();
    public Login_Usuario_DTO Usuario
    {
        get => usuario;
        set
        {
            usuario = value;
            OnPropertyChanged(nameof(Usuario));
        }
    }

    private Usuario_DTO usuarioLogado = new();
    public Usuario_DTO UsuarioLogado
    {
        get => usuarioLogado;
        set
        {
            usuarioLogado = value;
            OnPropertyChanged(nameof(UsuarioLogado));
        }
    }

    public ICommand FazerLoginCommand => new Command(async () =>
    {
        if (string.IsNullOrEmpty(Usuario.Telefone) || string.IsNullOrEmpty(Usuario.Senha))
        {
            await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }
        ActivityCommand.Execute(null);
        var json = JsonSerializer.Serialize(Usuario, options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("login/usuario", content);
        if (response.IsSuccessStatusCode)
        {
            //var result = await response.Content.ReadAsStringAsync();
            using var result = await response.Content.ReadAsStreamAsync();
            UsuarioLogado = JsonSerializer.Deserialize<Usuario_DTO>(result, options)!;
            //await SecureStorage.SetAsync("usuario", result);
            await SecureStorage.SetAsync("usuarioId", UsuarioLogado.Id.ToString());
            await SecureStorage.SetAsync("usuarioTipo", UsuarioLogado.IdTipo.ToString());
            await SecureStorage.SetAsync("usuarioNome", UsuarioLogado.Nome.ToString());
            await SecureStorage.SetAsync("usuarioTelefone", UsuarioLogado.Telefone.ToString());
            
            if (UsuarioLogado != null)
            {
                if (UsuarioLogado.IdTipo != 1)
                {
                    ActivityCommand.Execute(null);
                    await Shell.Current.GoToAsync("//Home_OfficePage");
                }
                else
                {
                    ActivityCommand.Execute(null);
                    // Navegar para a página inicial ou outra ação
                }
            }
        }
        else
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Erro", "Falha ao realizar login.", "OK");
        }
    });

    public ICommand NavegarParaCadastroCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Conta_ClientPage");
    });
    public ICommand NavegarParaRecuperarSenhaCommand => new Command(async () =>
    {
        await Shell.Current.GoToAsync("Senha_ClientPage");
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
