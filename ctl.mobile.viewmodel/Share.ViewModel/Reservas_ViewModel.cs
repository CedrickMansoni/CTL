using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using ctl.mobile.viewmodel.Client.ViewModel;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Marcacao;

namespace ctl.mobile.viewmodel.Share.ViewModel;

public class Reservas_ViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;
    private Timer _timer;
    public Reservas_ViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{Dominio.URLApp}") };
        options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var tipo = SecureStorage.Default.GetAsync("usuarioTipo");
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _timer = new Timer(_ =>
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Hora = DateTime.Now.ToString("T");
                    if (Convert.ToDateTime(hora).Second == 5  ||
                        Convert.ToDateTime(hora).Second == 10 ||
                        Convert.ToDateTime(hora).Second == 15 ||
                        Convert.ToDateTime(hora).Second == 20 ||
                        Convert.ToDateTime(hora).Second == 25 ||
                        Convert.ToDateTime(hora).Second == 30 ||
                        Convert.ToDateTime(hora).Second == 35 ||
                        Convert.ToDateTime(hora).Second == 40 ||
                        Convert.ToDateTime(hora).Second == 45 ||
                        Convert.ToDateTime(hora).Second == 50 ||
                        Convert.ToDateTime(hora).Second == 55 ||
                        Convert.ToDateTime(hora).Second == 59 )
                    {
                        if (!"1".Equals(tipo))
                        {
                            ListarTodasMarcacoesCommand.Execute(null);
                        }
                    }
                });
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        });
    }

    private string hora = string.Empty;
    public string Hora
    {
        get => hora;
        set
        {
            hora = value;
            OnPropertyChanged(nameof(Hora));
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
        int usuarioID = Convert.ToInt32(await SecureStorage.Default.GetAsync("usuarioId") ?? 0.ToString());

        int p = Marcacao.Count == 0 ? 0 : 1;
        var response = await client.GetAsync($"listar/marcacoes/usuario?idUsuario={usuarioID}&skip={Marcacao.Count + 30 * p}&take={30}");
        if (!response.IsSuccessStatusCode) return;
        using var m = await response.Content.ReadAsStreamAsync();
        var data = JsonSerializer.Deserialize<ObservableCollection<Listar_Marcacao_DTO>>(m, options) ?? [];

        if (data == null) return;

        //Filtra marcações válidas
        var hoje = DateTime.Now.Date;
        int ano = hoje.Year;
        int mes = hoje.Month;
        int dia = hoje.Day;
        var horaAtual = DateTime.Now.Date.Hour;

        if (Marcacao.Count == 0)
        {
            foreach (var item in data)
            {
                if (
                    item.DataMarcacao.Year >= ano &&
                    item.DataMarcacao.Month >= mes &&
                    item.DataMarcacao.Day >= dia &&
                    item.DataTermino.Hour > horaAtual
                    )
                {
                    Marcacao.Add(item);
                }
            }
            return;
        }

        // Adiciona apenas os que ainda não estão em Campos
        var novasNoticias = data.Except(Marcacao, new ListarMarcacaoDtoComparer()).ToList();
        foreach (var n in novasNoticias)
        {
            if (n.DataMarcacao.Date == DateTime.Now.Date)
            {
                await Shell.Current.DisplayAlert("Alerta", $"{n.Cliente}", "Ok");
                Marcacao.Insert(0, n);
            }
        }
    });
    public ICommand ListarTodasMarcacoesCommand => new Command(async () =>
    {
        int p = Marcacao.Count == 0 ? 0 : 1;
        var response = await client.GetAsync($"listar/marcacoes?skip={Marcacao.Count + 30 * p}&take={30}");
        if (!response.IsSuccessStatusCode) return;
        using var m = await response.Content.ReadAsStreamAsync();
        var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Listar_Marcacao_DTO>>(m, options) ?? [];
        if (data == null) return;

        //Filtra marcações válidas
        var hoje = DateTime.Now.Date;
        int ano = hoje.Year;
        int mes = hoje.Month;
        int dia = hoje.Day;
        var horaAtual = DateTime.Now.Date.Hour;

        if (Marcacao.Count == 0)
        {
            foreach (var item in data)
            {
                if (
                    item.DataMarcacao.Year >= ano &&
                    item.DataMarcacao.Month >= mes &&
                    item.DataMarcacao.Day >= dia &&
                    item.DataTermino.Hour > horaAtual
                    )
                {
                    Marcacao.Add(item);
                }
            }
            return;
        }

        if (Marcacao.Count != data.Count)
        {
            // Adiciona apenas os que ainda não estão em Campos
            var novasNoticias = data.Except(Marcacao, new ListarMarcacaoDtoComparer()).ToList();
            foreach (var item in novasNoticias)
            {
                if (
                        item.DataMarcacao.Year >= ano &&
                        item.DataMarcacao.Month >= mes &&
                        item.DataMarcacao.Day >= dia &&
                        item.DataTermino.Hour > horaAtual
                    )
                {
                    Marcacao.Insert(0, item);
                }
            }
        }
    });

    public ICommand AbrirCameraCommand => new Command<Listar_Marcacao_DTO>(async m =>
       {
           if (!"Pendente".Equals(m.Observacao)) return;
           bool response = await Shell.Current.DisplayAlert("...", "Como pretende obter a carregar o comprovativo?", "Camera", "Galeria");
           var doc = response ?
               await MediaPicker.CapturePhotoAsync() :
               await MediaPicker.PickPhotoAsync();

           if (doc is null) return;

           CaminhoImagem = doc.FullPath;
           NomeFile = doc.FileName;
           ColorFile = "file_green";

           bool salvar = await Shell.Current.DisplayAlert("...", "Deseja enviar o comprovativo selecionado?", "Enviar", "Cancelar");

           if (!salvar) return;

           EnviarComprovativoCommand.Execute(m);
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

    public ICommand EnviarComprovativoCommand => new Command<Listar_Marcacao_DTO>(async m =>
    {
        if (string.IsNullOrEmpty((CaminhoImagem)))
        {
            await Shell.Current.DisplayAlert("Atenção", "Selecione um comprovativo", "OK");
            return;
        }

        ActivityCommand.Execute(null);
        var telefone = await SecureStorage.GetAsync("usuarioTelefone");
        var comprovante = new Salvar_Comprovativo_DTO()
        {
            IdMarcacao = m.Id,
            Telefone = telefone!
        };

        var formData = new MultipartFormDataContent
        {
            { new StringContent(comprovante.IdMarcacao.ToString()), "idMarcacao" },
            { new StringContent(comprovante.Telefone), "telefone" }
        };

        AdicionarArquivoAoFormData(formData, CaminhoImagem, "comprovativo");

        var response = await client.PostAsync("upload/comprovativo", formData);
        if (response.IsSuccessStatusCode)
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Sucesso", "Comprovativo enviado com sucesso", "OK");
            Marcacao.Clear();
            ListarMarcacoesCommand.Execute(null);
        }
        else
        {
            ActivityCommand.Execute(null);
            await Shell.Current.DisplayAlert("Erro", "Erro ao enviar o comprovativo", "OK");
        }
    });

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

