using System;

namespace ctl.share.Dominio_App;

public static class Dominio
{
    public static string URLApp { get; set; }
    static Dominio()
    {
        URLApp = "https://52ca-105-172-230-207.ngrok-free.app/";
    }
}
