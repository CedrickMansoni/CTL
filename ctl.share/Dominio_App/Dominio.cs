using System;

namespace ctl.share.Dominio_App;

public static class Dominio
{
    public static string URLApp { get; private set; }
    static Dominio()
    {
        URLApp = "http://192.168.1.82:5130/";
    }
}
