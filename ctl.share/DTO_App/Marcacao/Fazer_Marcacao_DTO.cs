using System;
using Microsoft.AspNetCore.Http;

namespace ctl.share.DTO_App.Marcacao;

public class Fazer_Marcacao_DTO
{
    public int IdCliente { get; set; }
    public int IdCampo { get; set; }
    public DateTime DataMarcacao { get; set; } 
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
}
