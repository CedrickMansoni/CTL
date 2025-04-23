using System;
using System.Text.Json.Serialization;

namespace ctl.share.SMS_App;

public class Mensagem
{
    [JsonPropertyName("api_key_app")]
    public string ApiKeyApp { get; set; } = "prd7ee819ee23ed4ae5c16ec49952";

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("message_body")]
    public string MessageBody { get; set; } = string.Empty;
}
