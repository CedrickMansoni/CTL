using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ctl.webapi.Models;

[Table("t_01_tipo_usuario")]
public class TipoUsuarioModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("descricao")]
    public string Descricao { get; set; } = string.Empty;
}
