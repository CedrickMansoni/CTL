using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ctl.webapi.Models;

[Table("t_04_marcacao")]
public class MarcacaoModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("id_campo")]
    public int IdCampo { get; set; }

    [Column("data_inicio")]
    public DateTime DataInicio { get; set; }

    [Column("data_termino")]
    public DateTime DataTermino { get; set; }

    [Column("comprovativo")]
    public string Comprovativo { get; set; } = string.Empty;
    
    [Column("codigo_transacao")]
    public string CodigoTransacao { get; set; } = string.Empty;

    [Column("observacao")]
    public string Observacao { get; set; } = string.Empty;

}
