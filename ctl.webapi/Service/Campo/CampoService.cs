using System;
using ctl.share.DTO_App.Campo;
using ctl.webapi.Models;
using ctl.webapi.Repository.Campo;

namespace ctl.webapi.Service.Campo;

public class CampoService(ICampoRepository repository) : ICampoService
{
    private readonly ICampoRepository _repository = repository;

    public async Task<bool> AbilitarCampo(Desativar_Campo_DTO campo)
    {
        return await _repository.AbilitarCampo(new CampoModel
        {
            Id = campo.Id,
            Estado = campo.Estado
        });
    }

    public async Task<string> AddCampo(Cadastrar_Campo_DTO campo)
    {
        if(string.IsNullOrEmpty(campo.Nome))
            return "Campo não pode ser nulo ou vazio!";
        if(campo.Preco < 0)
            return "Preço não pode ser negativo!";
        
        try
        {
            var campoModel = new CampoModel
            {
                Nome = campo.Nome,
                Estado = "Activo",
                Preco = campo.Preco
            };
            return await _repository.AddCampo(campoModel);
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar campo: {ex.Message}";
        }
    }

    public async Task<string> DeleteCampo(int id)
    {
        if (id <= 0)
            return "Campo não pode ser nulo ou vazio!";
        try
        {
            return await _repository.DeleteCampo(id);
        }
        catch (Exception ex)
        {
            return $"Erro ao remover campo: {ex.Message}";
        }
        
    }

    public async Task<IEnumerable<Listar_Campo_DTO>> GetAllCampos()
    {
        var campos = await _repository.GetAllCampos();
        return campos.Select(c => new Listar_Campo_DTO
        {
            Id = c.Id,
            Nome = c.Nome,
            Preco = c.Preco,
            Estado = c.Estado
        });
    }

    public async Task<string> UpdateCampo(Editar_Campo_DTO campo)
    {
        if (campo.Id <= 0)
            return "Campo não pode ser nulo ou vazio!";
        if (string.IsNullOrEmpty(campo.Nome))
            return "Campo não pode ser nulo ou vazio!";
        if (campo.Preco < 0)
            return "Preço não pode ser negativo!";

        try
        {
            var campoModel = new CampoModel
            {
                Id = campo.Id,
                Nome = campo.Nome,
                Preco = campo.Preco,
                Estado = campo.Estado
            };
            return await _repository.UpdateCampo(campoModel);
        }
        catch (Exception ex)
        {
            return $"Erro ao actualizar campo: {ex.Message}";
        }
    }
}
