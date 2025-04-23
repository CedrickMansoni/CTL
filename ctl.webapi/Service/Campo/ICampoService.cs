using System;
using ctl.share.DTO_App.Campo;

namespace ctl.webapi.Service.Campo;

public interface ICampoService
{
    Task<string> AddCampo(Cadastrar_Campo_DTO campo);
    Task<string> DeleteCampo(int id);
    Task<bool> AbilitarCampo(Desativar_Campo_DTO campo);
    Task<IEnumerable<Listar_Campo_DTO>> GetAllCampos();
    Task<string> UpdateCampo(Editar_Campo_DTO campo);
}
