using System;
using ctl.webapi.Data;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Repository.Banco;

public class ContaRepository(AppDataContext context) : IContaRepository
{
    private readonly AppDataContext _context = context;

    public async Task<string> AddAsync(ContaModel conta)
    {
        try
        {
            // Verifica se a conta já existe
            var existingConta = await _context.TabelaConta
                .FirstOrDefaultAsync(c => c.Numero == conta.Numero && c.IdBanco == conta.IdBanco);
            if (existingConta != null) return "Conta já existe!";
            await _context.TabelaConta.AddAsync(conta);
            await _context.SaveChangesAsync();
            return "Conta adicionada com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar conta: {ex.Message}";
        }
    }

    public async Task<string> DeleteAsync(int id)
    {
        try
        {
            var conta = await _context.TabelaConta.FindAsync(id);
            if (conta == null) return "Conta não encontrada!";
            _context.TabelaConta.Remove(conta);
            await _context.SaveChangesAsync();
            return "Conta removida com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao remover conta: {ex.Message}";
        }
    }

    public async Task<IEnumerable<ContaModel>> GetAllAsync()
    {
        return await _context.TabelaConta.ToListAsync();
    }

    public async Task<ContaModel> GetByIdAsync(int id)
    {
        var conta = await _context.TabelaConta.FindAsync(id);
        if (conta == null) throw new Exception("Conta não encontrada!");
        return conta;
    }

    public async Task<string> UpdateAsync(ContaModel conta)
    {
        try
        {
            var existingConta = await _context.TabelaConta.FindAsync(conta.Id);
            if (existingConta == null) return "Conta não encontrada!";
            existingConta.Numero = conta.Numero;
            existingConta.IdBanco = conta.IdBanco;
            existingConta.Iban = conta.Iban;
            _context.TabelaConta.Update(existingConta);
            await _context.SaveChangesAsync();
            return "Conta atualizada com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao atualizar conta: {ex.Message}";
        }
    }
}
