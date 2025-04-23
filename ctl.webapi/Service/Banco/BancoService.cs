using System;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Banco;
using ctl.webapi.Models;
using ctl.webapi.Repository.Banco;
using ctl.webapi.SalvarArquivos;

namespace ctl.webapi.Service.Banco;

public class BancoService(IBancoRepository repository, IArquivoService arquivo, IConfiguration configuration) : IBancoService
{
    private readonly IBancoRepository _repository = repository;
    private readonly IArquivoService _arquivo = arquivo;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> AddBancoAsync(Banco_DTO banco)
    {
        if (banco == null)
            throw new ArgumentNullException(nameof(banco));

        var result = await _repository.AddBancoAsync(new BancoModel
        {
            Id = banco.Id,
            Nome = banco.NomeAbreviado,
            Logo = banco.Logo!.FileName,
            Estado = banco.Estado
        });

        if (result.Contains("sucesso"))
        {
            
            string storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
            _configuration["VPSStoragePath:ProdutionStoragePath"]! :
            _configuration["StoragePath:LocalStoragePath"]!;

            await _arquivo.SalvarArquivoAsync(banco.Logo, storagePath, "Banco");
        }
        return result;
    }

    public async Task<string> DeleteBancoAsync(int id)
    {
        if (id <= 0)
            return "Id inválido";

        var result = await _repository.DeleteBancoAsync(id);
        return result;
    }

    public async Task<IEnumerable<Banco_Response_DTO>> GetAllBancosAsync()
    {
        var bancos = await _repository.GetAllBancosAsync();
        return bancos.Select(b => new Banco_Response_DTO
        {
            Id = b.Id,
            NomeAbreviado = b.NomeAbreviado,
            Logo = $"{Dominio.URLApp}/images/Banco/{b.Logo}", 
            Estado = b.Estado
        });
    }

    public async Task<Banco_Response_DTO?> GetBancoByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        var banco = await _repository.GetBancoByIdAsync(id);
        if (banco == null)
            return null;

        return new Banco_Response_DTO
        {
            Id = banco.Id,
            NomeAbreviado = banco.Nome,
            Logo = $"{Dominio.URLApp}/images/Banco/{banco.Logo}",
            Estado = banco.Estado
        };
    }

    public async Task<string> UpdateBancoAsync(Banco_DTO banco)
    {
        if (banco == null)
            throw new ArgumentNullException(nameof(banco));

        var result = await _repository.UpdateBancoAsync(new BancoModel
        {
            Id = banco.Id,
            Nome = banco.NomeAbreviado,
            Logo = banco.Logo!.FileName,
            Estado = banco.Estado
        });
        return result;
    }
}
