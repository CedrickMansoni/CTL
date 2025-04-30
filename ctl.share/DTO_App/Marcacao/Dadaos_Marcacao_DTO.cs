using System;

namespace ctl.share.DTO_App.Marcacao;

public class Dados_Marcacao_DTO
{
    public string NomeCliente { get; set; } = string.Empty;
    public string NomeCampo { get; set; } = string.Empty;
    public DateTime DataMarcacao { get; set; } 
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string StatusMarcacao { get; set; }  = string.Empty;
}
