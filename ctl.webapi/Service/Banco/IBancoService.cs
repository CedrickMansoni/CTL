using System;
using ctl.share.DTO_App.Banco;

namespace ctl.webapi.Service.Banco;

public interface IBancoService
{
     Task<string> AddBancoAsync(Banco_DTO banco);
    Task<string> UpdateBancoAsync(Banco_DTO banco);
    Task<string> DeleteBancoAsync(int id);
    Task<Banco_Response_DTO?> GetBancoByIdAsync(int id);
    Task<IEnumerable<Banco_Response_DTO>> GetAllBancosAsync();
}
