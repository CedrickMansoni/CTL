using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ctl.webapi.Models;

[Table("t_08_conta")]
public class ContaModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_banco")]
    public int IdBanco { get; set; }

    [Column("numero")]
    public string Numero { get; set; } = string.Empty;

    [Column("iban")]
    public string Iban { get; set; } = string.Empty;
}
