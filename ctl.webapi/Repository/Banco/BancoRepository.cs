using System;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Banco;
using ctl.webapi.Data;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Repository.Banco;

public class BancoRepository(AppDataContext context) : IBancoRepository
{
    private readonly AppDataContext _context = context;

    public async Task<string> AddBancoAsync(BancoModel banco, ContaModel conta)
    {
        var transacao = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingBanco = await _context.TabelaBanco
                .FirstOrDefaultAsync(b => b.Nome == banco.Nome);
            if (existingBanco != null) return "Banco já existe.";
            await _context.TabelaBanco.AddAsync(banco);
            await _context.SaveChangesAsync();

            conta.IdBanco = banco.Id;
            await _context.TabelaConta.AddAsync(conta);
            await _context.SaveChangesAsync(); 
            transacao.Commit();
            return "Banco adicionado com sucesso.";
        }
        catch (Exception ex)
        {
            await transacao.RollbackAsync();
            return $"Erro no cadastro do banco: {ex.Message}";
        }
    }

    public async Task<string> DeleteBancoAsync(int id)
    {
        try
        {
            var banco = await _context.TabelaBanco.FindAsync(id);
            if (banco == null) return "Banco não encontrado.";
            _context.TabelaBanco.Remove(banco);
            await _context.SaveChangesAsync();
            return "Banco excluído com sucesso.";
        }
        catch 
        {
            return $"Erro na exclusão do banco";
        }
    }

    public async Task<IEnumerable<Banco_Response_DTO>> GetAllBancosAsync()
    {
        var bancos = from b in _context.TabelaBanco
                     join c in _context.TabelaConta on b.Id equals c.IdBanco
                     select new Banco_Response_DTO
                     {
                         Id = b.Id,
                         NomeAbreviado = b.Nome,
                         Conta = c.Numero,
                         IBAN = c.Iban,
                         Logo = $"{Dominio.URLApp}images/Banco/{b.Logo}",
                     };
        return await bancos.ToListAsync();
    }

    public async Task<BancoModel?> GetBancoByIdAsync(int id)
    {
        return await _context.TabelaBanco
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<string> UpdateBancoAsync(BancoModel banco)
    {
        try
        {
            var existingBanco = await _context.TabelaBanco
                .FirstOrDefaultAsync(b => b.Id == banco.Id);
            if (existingBanco == null) return "Banco não encontrado.";
            existingBanco.Nome = banco.Nome;
            existingBanco.Logo = banco.Logo;
            await _context.SaveChangesAsync();
            return "Banco actualizado com sucesso.";
        }
        catch (Exception ex)
        {
            return $"Erro na actualização do banco: {ex.Message}";
        }
    }
}
