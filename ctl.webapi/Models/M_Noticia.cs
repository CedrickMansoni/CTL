using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ctl.webapi.Models;
[Table("t_06_noticia")]
public class NoticiaModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_funcionario")]
    public int IdUsuario { get; set; }

    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("materia")]
    public string Materia { get; set; } = string.Empty;

    [Column("data_publicacao")]
    public DateTime DataPublicacao { get; set; }

    [Column("imagem")]
    public string Imagem { get; set; } = string.Empty;
}