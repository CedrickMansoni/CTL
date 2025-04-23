using System;
using ctl.share.DTO_App.Noticia;

namespace ctl.webapi.Service.Noticia;

public interface INoticiaService
{
    Task<IEnumerable<Noticia_DTO>> GetAll();
    Task<string> Add(Noticia_DTO noticia);
    Task<string> Update(Noticia_DTO noticia);
    Task<string> Delete(int id);
}
