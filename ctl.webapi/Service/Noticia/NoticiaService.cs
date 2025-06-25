using System;
using ctl.share.DTO_App.Noticia;
using ctl.webapi.Repository.Noticia;
using ctl.webapi.SalvarArquivos;

namespace ctl.webapi.Service.Noticia;

public class NoticiaService(INoticiaRepository repository, IConfiguration configuration, IArquivoService arquivo) : INoticiaService
{

    private readonly INoticiaRepository _repository = repository;
    private readonly IArquivoService _arquivo = arquivo;
    private readonly IConfiguration _configuration = configuration;
    public async Task<string> Add(Noticia_DTO noticia)
    {
        string storagePath;
        storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
        _configuration["VPSStoragePath:ProdutionStoragePath"]! :
        _configuration["StoragePath:LocalStoragePath"]!;
        var result = await _repository.Add(new Models.NoticiaModel
        {
            IdUsuario = noticia.IdUsuario,
            Titulo = noticia.Titulo,
            Materia = noticia.Materia,
            DataPublicacao = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.Now), DateTimeKind.Utc),
            Imagem = noticia.Ficheiro.FileName,
        });

        if (result.Contains("sucesso"))
        {
            // Garantir que o diretório existe
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            await _arquivo.SalvarArquivoAsync(noticia.Ficheiro, storagePath, "noticia");
        }
        return result;
    }

    public async Task<string> Delete(int id)
    {
        var n = await _repository.GetById(id);
        if (n is null) return "A notícia que pretende apagar não existe na base de dados";
        var result = await _repository.Delete(id);
        if (result.Contains("sucesso"))
        {
            string storagePath;
            storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
            _configuration["VPSStoragePath:ProdutionStoragePath"]! :
            _configuration["StoragePath:LocalStoragePath"]!;
            string caminhoFile = Path.Combine(storagePath, "noticia", $"{n.Imagem}");
            if (File.Exists(caminhoFile))
            {
                File.Delete(caminhoFile);
            }
        }
        return result;
    }

    public async Task<IEnumerable<Noticia_DTO>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<string> Update(Noticia_DTO noticia)
    {
         string storagePath;
            storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
            _configuration["VPSStoragePath:ProdutionStoragePath"]! :
            _configuration["StoragePath:LocalStoragePath"]!;


        var n = await _repository.GetById(noticia.Id);
        if (n is null) return "A notícia que pretende editar não existe na base de dados";
        string caminhoFile = Path.Combine(storagePath, "noticia", $"{n.Imagem}");
        File.Delete(caminhoFile);

        var result = await _repository.Update(new Models.NoticiaModel
        {
            Id = noticia.Id,
            Titulo = noticia.Titulo,
            Materia = noticia.Materia,
            Imagem = noticia.Ficheiro.FileName,
        });
        if (result.Contains("sucesso"))
        {
            await _arquivo.SalvarArquivoAsync(noticia.Ficheiro, storagePath, "noticia");

        }
        return result;
    }
}
