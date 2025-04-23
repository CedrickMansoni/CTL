using System;
using ctl.share.DTO_App.Banco;

namespace ctl.webapi.Service.Banco;

public interface IContaService
{
    Task<IEnumerable<Conta_DTO>> GetAllAsync();
    Task<string> AddAsync(Conta_DTO conta);
    Task<string> UpdateAsync(Conta_DTO conta);
    Task<string> DeleteAsync(int id);
}
