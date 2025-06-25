using System;
using ctl.webapi.Data;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Repository.Campo;

public class CampoRepository(AppDataContext context) : ICampoRepository
{
    private readonly AppDataContext _context = context;

    public async Task<bool> AbilitarCampo(CampoModel campo)
    {
        var campoDb = await _context.TabelaCampo.FindAsync(campo.Id);
        if (campoDb == null)
            return false;

        campoDb.Estado = campo.Estado;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> AddCampo(CampoModel campo)
    {
        try
        {
            var campoDb = await _context.TabelaCampo.FirstOrDefaultAsync(x => x.Nome == campo.Nome);
            if (campoDb != null)
                return "Campo já existe!";
            await _context.TabelaCampo.AddAsync(campo);
            await _context.SaveChangesAsync();
            return "Campo adicionado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar campo: {ex.Message}";
        }
    }

    public async Task<string> DeleteCampo(int id)
    {
        try
        {
            var campoDb = await _context.TabelaCampo.FindAsync(id);
            if (campoDb == null)
                return "Campo não encontrado!";
            _context.TabelaCampo.Remove(campoDb);
            await _context.SaveChangesAsync();
            return "Campo eliminado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao remover campo: {ex.Message}";
        }
    }

    public async Task<IEnumerable<CampoModel>> GetAllCampos()
    {
        return await _context.TabelaCampo.ToListAsync();
    }

    public async Task<string> UpdateCampo(CampoModel campo)
    {
        try
        {
            var campoDb = await _context.TabelaCampo.FindAsync(campo.Id);
            if (campoDb == null)
                return "Campo não encontrado!";
            campoDb.Nome = campo.Nome;
            campoDb.Preco = campo.Preco;
            campoDb.Estado = campo.Estado;
            await _context.SaveChangesAsync();
            return "Campo actualizado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao actualizar campo: {ex.Message}";
        }
    }
}
