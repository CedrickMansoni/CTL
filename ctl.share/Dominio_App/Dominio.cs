using System;

namespace ctl.share.Dominio_App;

public static class Dominio
{
    public static string URLApp { get; set; }
    static Dominio()
    {
        URLApp = "https://58ba-129-122-151-81.ngrok-free.app/";
    }
}
