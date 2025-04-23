using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ctl.webapi.Models;
[Table("t_05_estado_marcacao")]
public class EstadoMarcacaoModel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_funcionario")]
    public int IdFuncionario { get; set; }
    
    [Column("id_marcacao")]
    public int IdMarcacao { get; set; }
}
