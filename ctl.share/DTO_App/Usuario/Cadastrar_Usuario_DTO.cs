using System;

namespace ctl.share.DTO_App.Usuario;

public class Cadastrar_Usuario_DTO
{

    public int IdTipo { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

}
