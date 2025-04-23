using System;

namespace ctl.share.SMS_App;

public interface ISMS_enviar
{
    Task<MensagemResponse?> EnviarSMS(EnviarMensagem mensagem);
}
