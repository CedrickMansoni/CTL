using System;
using ctl.share.DTO_App.Banco;
using ctl.webapi.Models;
using ctl.webapi.Repository.Banco;

namespace ctl.webapi.Service.Banco;

public class ContaService(IContaRepository repository) : IContaService
{
    private readonly IContaRepository _repository = repository;
    public async Task<string> AddAsync(Conta_DTO conta)
    {
        if (string.IsNullOrEmpty(conta.NumeroConta) || string.IsNullOrEmpty(conta.IBAN)) return "Número da conta ou IBAN inválido";


        var result = await _repository.AddAsync(new ContaModel
        {
            Numero = conta.NumeroConta,
            Iban = conta.IBAN,
        });
        return result;
    }

    public async Task<string> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Conta_DTO>> GetAllAsync()
    {
        var contas = await _repository.GetAllAsync();
        return contas.Select(c => new Conta_DTO
        {
            Id = c.Id,
            NumeroConta = c.Numero,
            IBAN = c.Iban,
        });
    }


    public async Task<string> UpdateAsync(Conta_DTO conta)
    {
        if (string.IsNullOrEmpty(conta.NumeroConta) || string.IsNullOrEmpty(conta.IBAN)) return "Número da conta ou IBAN inválido";

        var result = await _repository.UpdateAsync(new ContaModel
        {
            Id = conta.Id,
            Numero = conta.NumeroConta,
            Iban = conta.IBAN,
        });
        return result;
    }
}
