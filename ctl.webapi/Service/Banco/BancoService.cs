using System;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Banco;
using ctl.webapi.Models;
using ctl.webapi.Repository.Banco;
using ctl.webapi.SalvarArquivos;

namespace ctl.webapi.Service.Banco;

public class BancoService(IBancoRepository repository, IArquivoService arquivo, IConfiguration configuration, IContaService service) : IBancoService
{
    private readonly IBancoRepository _repository = repository;
    private readonly IArquivoService _arquivo = arquivo;
    private readonly IConfiguration _configuration = configuration;
    private readonly IContaService _service = service;

    public async Task<string> AddBancoAsync(Banco_DTO banco, Conta_DTO conta)
    {
        if (banco == null) return "Banco inválido";
        if (conta == null) return "Conta inválido";

        var result = await _repository.AddBancoAsync(new BancoModel
        {
            Id = banco.Id,
            Nome = banco.NomeAbreviado,
            Logo = banco.Logo!.FileName,
            Estado = banco.Estado
        }, new ContaModel
        {
            Numero = conta.NumeroConta!,
            Iban = conta.IBAN!
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
        string storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
           _configuration["VPSStoragePath:ProdutionStoragePath"]! :
           _configuration["StoragePath:LocalStoragePath"]!;


        if (id <= 0)
            return "Id inválido";

        var b = await _repository.GetBancoByIdAsync(id);

        if (b is null) return "O banco que pretende apagar não existe no banco.";
        File.Delete(Path.Combine(storagePath, "Banco", $"{b.Logo}"));

        var result = await _repository.DeleteBancoAsync(id);

        return result;
    }

    public async Task<IEnumerable<Banco_Response_DTO>> GetAllBancosAsync()
    {
        return await _repository.GetAllBancosAsync();
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
        if (banco == null) return "Selecione o banco que pretende editar";

        string storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
           _configuration["VPSStoragePath:ProdutionStoragePath"]! :
           _configuration["StoragePath:LocalStoragePath"]!;


        var b = await _repository.GetBancoByIdAsync(banco.Id);

        if (b is null) return "O banco que pretende apagar não existe no banco.";
        File.Delete(Path.Combine(storagePath, "Banco", $"{b.Logo}"));

        var result = await _repository.UpdateBancoAsync(new BancoModel
        {
            Id = banco.Id,
            Nome = banco.NomeAbreviado,
            Logo = banco.Logo!.FileName,
            Estado = banco.Estado
        });

        await _service.UpdateAsync(new Conta_DTO { IdBanco = banco.Id, IBAN = banco.IBAN, NumeroConta = banco.Conta });

        if (result.Contains("sucesso"))
        {
            await _arquivo.SalvarArquivoAsync(banco.Logo, storagePath, "Banco");
        }
        return result;
    }
}
