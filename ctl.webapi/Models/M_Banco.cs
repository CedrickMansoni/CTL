using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ctl.webapi.Models;

[Table("t_07_banco")]
public class BancoModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("logo")]
    public string Logo { get; set; } = string.Empty;

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
}
