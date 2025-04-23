using System;
using ctl.webapi.Models;

namespace ctl.webapi.Repository.Banco;

public interface IContaRepository
{
    Task<ContaModel> GetByIdAsync(int id);
    Task<IEnumerable<ContaModel>> GetAllAsync();
    Task<string> AddAsync(ContaModel conta);
    Task<string> UpdateAsync(ContaModel conta);
    Task<string> DeleteAsync(int id);
}
