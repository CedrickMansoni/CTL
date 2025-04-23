using System;

namespace ctl.share.DTO_App.Usuario;

public class Editar_Usuario_DTO
{
    public int Id { get; set; }
    public int IdTipo { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string SenhaAntiga { get; set; } = string.Empty;
    public string SenhaNova { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
