using System;

namespace ctl.share.DTO_App.Banco;

public class Conta_DTO
{
    public int Id { get; set; }
    public int IdBanco { get; set; }
    public string? IBAN { get; set; }
    public string? NomeTitular { get; set; }
    public string? NumeroConta { get; set; }
}
