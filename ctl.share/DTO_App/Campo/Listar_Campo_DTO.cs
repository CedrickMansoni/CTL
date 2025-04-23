using System;

namespace ctl.share.DTO_App.Campo;

public class Listar_Campo_DTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Estado { get; set; } = string.Empty;
}
