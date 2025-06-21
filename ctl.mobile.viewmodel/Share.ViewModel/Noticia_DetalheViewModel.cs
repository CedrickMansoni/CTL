using System;
using System.Text.Json;
using ctl.share.DTO_App.Noticia;

namespace ctl.mobile.viewmodel.Share.ViewModel;

[QueryProperty(nameof(NoticiaJSON), "noticia")]
public class Noticia_DetalheViewModel : BindableObject
{
    private string noticiaJSON = string.Empty;
    public string NoticiaJSON
    {
        get { return noticiaJSON; }
        set
        {
            noticiaJSON = value;
            if (!string.IsNullOrEmpty(noticiaJSON))
            {
                Noticia = JsonSerializer.Deserialize<Noticia_DTO>(noticiaJSON) ?? new Noticia_DTO();
            }
        }
    }

    private Noticia_DTO noticia = new();
    public Noticia_DTO Noticia
    {
        get { return noticia; }
        set
        {
            noticia = value;
            OnPropertyChanged(nameof(Noticia));
        }
    }
}
