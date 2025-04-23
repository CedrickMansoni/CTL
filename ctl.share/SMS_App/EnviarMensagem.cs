using System;
using System.Text.Json.Serialization;

namespace ctl.share.SMS_App;

public class EnviarMensagem
{
    [JsonPropertyName("message")]
    public required Mensagem Mensagem { get; set; }
}
