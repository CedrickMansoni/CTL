using System;
using Microsoft.AspNetCore.Http;

namespace ctl.share.DTO_App.Marcacao;

public class Salvar_Comprovativo_DTO
{
    public int IdMarcacao { get; set; }
    public string CodigoTransacao { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public IFormFile? Comprovativo { get; set; }
}
