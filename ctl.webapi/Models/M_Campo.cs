using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ctl.webapi.Models;

[Table("t_03_campo")]
public class CampoModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("preco")]
    public decimal Preco { get; set; }

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
}
