using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ctl.webapi.Models;

[Table("t_02_usuario")]
public class UsuarioModel

{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_tipo")]
    public int IdTipo { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("telefone")]
    public string Telefone { get; set; } = string.Empty;

    [Column("senha")]
    public string Senha { get; set; } = string.Empty;

    [Column("estado")]
    public string Estado { get; set; } = string.Empty;
}
