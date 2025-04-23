using System;
using System.Text.Json.Serialization;

namespace ctl.share.SMS_App;

public class MensagemResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}
