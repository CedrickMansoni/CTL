using System;

namespace ctl.share.DTO_App.Marcacao;

public class Alterar_Marcacao_DTO
{
    public int IdMarcacao { get; set; }
    public DateTime DataMarcacao { get; set; } 

    public DateTime DataInicio { get; set; }

    public DateTime DataTermino { get; set; }
}
