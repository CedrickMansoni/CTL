using System;
using Microsoft.AspNetCore.Http;

namespace ctl.share.DTO_App.Banco;

public class Banco_DTO
{
    public int Id { get; set; }
    public string NomeAbreviado { get; set; } = string.Empty;
    public IFormFile? Logo { get; set; }
    public string Estado { get; set; } = "Activo";
}
