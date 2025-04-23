using System;
using System.Text;
using System.Text.Json;

namespace ctl.share.SMS_App;

public class SMS_enviar : ISMS_enviar
{

    HttpClient client;
    JsonSerializerOptions option;
    public SMS_enviar()
    {
        client = new HttpClient { BaseAddress = new Uri("https://www.telcosms.co.ao/") };
        option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }
    public async Task<MensagemResponse?> EnviarSMS(EnviarMensagem mensagem)
    {
        string json = JsonSerializer.Serialize<EnviarMensagem>(mensagem, option);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/v2/send_message", content);

        MensagemResponse smsResponse = new();
        if (response.IsSuccessStatusCode)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<MensagemResponse>(responseStream, option);
            }
        }
        return null;
    }
}
