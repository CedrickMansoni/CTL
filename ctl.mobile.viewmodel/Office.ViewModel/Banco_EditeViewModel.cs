using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Banco;

namespace ctl.mobile.viewmodel.Office.ViewModel;

[QueryProperty(nameof(BancoJSON), "banco")]
public class Banco_EditeViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;
    public Banco_EditeViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private Banco_DTO banco = new();
    public Banco_DTO Banco
    {
        get => banco;
        set
        {
            banco = value;
            OnPropertyChanged(nameof(Banco));
        }
    }

    private string bancoJSON = string.Empty;
    public string BancoJSON
    {
        get => bancoJSON;
        set
        {
            bancoJSON = value;
            if (!string.IsNullOrEmpty(BancoJSON))
            {
                var banc = JsonSerializer.Deserialize<Banco_Response_DTO>(BancoJSON) ?? new();
                var banc2 = new Banco_DTO
                {
                    Id = banc.Id,
                    NomeAbreviado = banc.NomeAbreviado,
                    Conta = banc.Conta,
                    IBAN = banc.IBAN
                };
                Banco = banc2;
            }
        }
    }

    private string caminhoImagem = string.Empty;
    public string CaminhoImagem
    {
        get => caminhoImagem;
        set
        {
            caminhoImagem = value;
            OnPropertyChanged(nameof(CaminhoImagem));
        }
    }

    private string nomeFile = string.Empty;
    public string NomeFile
    {
        get => nomeFile;
        set
        {
            nomeFile = value;
            OnPropertyChanged(nameof(NomeFile));
            ColorFile = string.IsNullOrEmpty(nomeFile) ? "file" : "file_green";
        }
    }

    private string colorFile = "file";
    public string ColorFile
    {
        get => colorFile;
        set
        {
            colorFile = value;
            OnPropertyChanged(nameof(ColorFile));
        }
    }

    public ICommand EditarBancoCommand => new Command(async () =>
    {
        if (string.IsNullOrEmpty(Banco.NomeAbreviado))
        {
            await Shell.Current.DisplayAlert("Erro", "Preencha todos os campos", "OK");
            return;
        }
        if (string.IsNullOrEmpty(CaminhoImagem))
        {
            await Shell.Current.DisplayAlert("Erro", "Selecione uma imagem", "OK");
            return;
        }

        ActivityCommand.Execute(null);
        var formData = new MultipartFormDataContent
        {
            { new StringContent(Banco.Id.ToString()), "id" },
            { new StringContent(Banco.NomeAbreviado.ToString()), "nome" },
            { new StringContent(Banco.Estado.ToString()), "estado" },
            { new StringContent(Banco.Conta!.ToString()), "conta" },
            { new StringContent(Banco.IBAN!.ToString()), "iban" },
        };

        AdicionarArquivoAoFormData(formData, CaminhoImagem, "logo");

        var response = await client.PutAsync($"editar/banco", formData);
        if (response.IsSuccessStatusCode)
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Sucesso", "Banco editado com sucesso", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            ActivityCommand.Execute(null);
            var errorMessage = await response.Content.ReadAsStringAsync();
            await Shell.Current.DisplayAlert("Erro", $"{errorMessage}", "OK");
        }
    });

    public ICommand AbrirCameraCommand => new Command(async () =>
    {
        bool response = await Shell.Current.DisplayAlert("...", "Como pretende obter a logo do banco?", "Camera", "Galeria");
        var doc = response ?
            await MediaPicker.CapturePhotoAsync() :
            await MediaPicker.PickPhotoAsync();

        if (doc is null) return;

        CaminhoImagem = doc.FullPath;
        NomeFile = doc.FileName;
        ColorFile = "file_green";

    });




    // Método para adicionar arquivos ao formData
    private void AdicionarArquivoAoFormData(MultipartFormDataContent formData, string caminho, string nomeCampo)
    {
        if (!string.IsNullOrEmpty(caminho) && File.Exists(caminho))
        {
            var mimeType = ObterTipoMime(caminho);
            var imagemStream = new StreamContent(File.OpenRead(caminho));
            imagemStream.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            formData.Add(imagemStream, nomeCampo, Path.GetFileName(caminho));
        }
    }

    // Método auxiliar para determinar o tipo MIME com base na extensão do arquivo
    private string ObterTipoMime(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream", // Padrão para tipos desconhecidos
        };
    }


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
