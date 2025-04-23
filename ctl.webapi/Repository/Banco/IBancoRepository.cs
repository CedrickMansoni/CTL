using System;
using ctl.share.DTO_App.Banco;
using ctl.webapi.Models;

namespace ctl.webapi.Repository.Banco;

public interface IBancoRepository
{
    Task<string> AddBancoAsync(BancoModel banco);
    Task<string> UpdateBancoAsync(BancoModel banco);
    Task<string> DeleteBancoAsync(int id);
    Task<BancoModel?> GetBancoByIdAsync(int id);
    Task<IEnumerable<Banco_Response_DTO>> GetAllBancosAsync();
}
