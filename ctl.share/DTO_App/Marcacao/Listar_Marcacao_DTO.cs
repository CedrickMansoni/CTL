using System;

namespace ctl.share.DTO_App.Marcacao;

public class Listar_Marcacao_DTO
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public int IdCampo { get; set; }
    public string Campo { get; set; } = string.Empty;
    public DateTime DataMarcacao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public string Comprovativo { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    public decimal ValorPagamento { get; set; }
    public bool EstadoMarcacao { get; set; }

}
