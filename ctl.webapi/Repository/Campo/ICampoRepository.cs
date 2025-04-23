using System;
using ctl.webapi.Models;

namespace ctl.webapi.Repository.Campo;

public interface ICampoRepository
{
    Task<string> AddCampo(CampoModel campo);
    Task<string> DeleteCampo(int id);
    Task<bool> AbilitarCampo(CampoModel campo);
    Task<IEnumerable<CampoModel>> GetAllCampos();
    Task<string> UpdateCampo(CampoModel campo);
}
