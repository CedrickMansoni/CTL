using System;

namespace ctl.share.Dominio_App;

public static class Dominio
{
    public static string URLApp { get; private set; }
    static Dominio()
    {
        URLApp = "http://172.20.10.2:5130/";
    }
}
