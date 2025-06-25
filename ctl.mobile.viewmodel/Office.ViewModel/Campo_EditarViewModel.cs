using System;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Campo;

namespace ctl.mobile.viewmodel.Office.ViewModel;

[QueryProperty(nameof(CampoJSON), "campo")]
public class Campo_EditarViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;

    public Campo_EditarViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private string campoJSON = string.Empty;
    public string CampoJSON
    {
        get => campoJSON;
        set
        {
            campoJSON = value;
            if (!string.IsNullOrEmpty(campoJSON))
            {
                Campo = JsonSerializer.Deserialize<Editar_Campo_DTO>(campoJSON) ?? new();
            }
        }
    }

    private Editar_Campo_DTO campo = new();
    public Editar_Campo_DTO Campo
    {
        get => campo;
        set
        {
            campo = value;
            OnPropertyChanged(nameof(Campo));
        }
    }

    public ICommand EditarCampoCommand => new Command(async () =>
    {
        if (string.IsNullOrWhiteSpace(Campo.Nome))
        {
            await Shell.Current.DisplayAlert("Error", "O campo não pode ser vazio", "OK");
            return;
        }
        if (Campo.Preco <= 0)
        {
            await Shell.Current.DisplayAlert("Error", "O preço deve ser maior que zero", "OK");
            return;
        }
        ActivityCommand.Execute(null);
        var json = JsonSerializer.Serialize(Campo, options);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PutAsync("editar/campo", content);

        if (response.IsSuccessStatusCode)
        {
            ActivityCommand.Execute(null);
            // Handle success
            await Shell.Current.DisplayAlert("Success", "Campo adicionado com sucesso", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            ActivityCommand.Execute(null);
            // Handle error
            await Shell.Current.DisplayAlert("Error", "Não foi possível adicionar o campo, por favor tente novamente, se o problema persistir entre em contacto com o time de suporte", "OK");
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

