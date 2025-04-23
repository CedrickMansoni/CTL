using System;

namespace ctl.share.DTO_App.Marcacao;

public class Cancelar_Marcacao_DTO
{
    public int IdMarcacao { get; set; }
    public string Observacao { get; set; } = string.Empty;
}
