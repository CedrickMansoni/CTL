using System;

namespace ctl.share.DTO_App.Banco;

public class Banco_Response_DTO
{
    public int Id { get; set; }
    public string NomeAbreviado { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo";

    public string Conta { get; set; } = string.Empty;
    public string IBAN { get; set; } = string.Empty;
}
