using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Noticia;

namespace ctl.mobile.viewmodel.Office.ViewModel;

public class NoticiaAdd_OfficeViewModel : BindableObject
{
    HttpClient client;
    JsonSerializerOptions options;
    public NoticiaAdd_OfficeViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private Noticia_DTO noticia = new();
    public Noticia_DTO Noticia
    {
        get { return noticia; }
        set
        {
            noticia = value;
            OnPropertyChanged(nameof(Noticia));
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

    public ICommand CadastrarNoticiaCommand => new Command(async () =>
    {
        if (string.IsNullOrEmpty(Noticia.Titulo))
        {
            await Shell.Current.DisplayAlert("Atenção", "Preencha o campo Titulo", "OK");
            return;
        }
        if (string.IsNullOrEmpty(Noticia.Materia))
        {
            await Shell.Current.DisplayAlert("Atenção", "Preencha o campo Descrição", "OK");
            return;
        }
        if (string.IsNullOrEmpty(CaminhoImagem))
        {
            await Shell.Current.DisplayAlert("Atenção", "Selecione uma imagem", "OK");
            return;
        }
        ActivityCommand.Execute(null);
        var idUsuario = await SecureStorage.GetAsync("usuarioId");
        noticia.IdUsuario = Convert.ToInt32(idUsuario);
        var formData = new MultipartFormDataContent
        {
            { new StringContent(Noticia.IdUsuario.ToString()), "idUsuario" },
            { new StringContent(Noticia.Titulo), "titulo" },
            { new StringContent(Noticia.Materia), "materia" },
        };

        AdicionarArquivoAoFormData(formData, CaminhoImagem, "imagem");

        var response = await client.PostAsync("adicionar/noticia", formData);
        if (response.IsSuccessStatusCode)
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Sucesso", "Noticia cadastrada com sucesso", "OK");
            await Shell.Current.GoToAsync("..");
        }else
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Erro", "Erro ao cadastrar noticia", "OK");
        }
    });


    public ICommand AbrirCameraCommand => new Command(async () =>
    {
        bool response = await Shell.Current.DisplayAlert("...", "Como pretende obter a foto da noticia?", "Camera", "Galeria");
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


