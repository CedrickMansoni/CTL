using System;

namespace ctl.webapi.SalvarArquivos;

public interface IArquivoService
{
    Task<string> SalvarArquivoAsync(IFormFile arquivo, string storagePath, string pasta);
}
