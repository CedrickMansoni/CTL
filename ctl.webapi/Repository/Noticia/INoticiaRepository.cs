using System;
using ctl.share.DTO_App.Noticia;
using ctl.webapi.Models;

namespace ctl.webapi.Repository.Noticia;

public interface INoticiaRepository
{
    Task<IEnumerable<Noticia_DTO>> GetAll();
    Task<NoticiaModel?> GetById(int id);
    Task<string> Add(NoticiaModel noticia);
    Task<string> Update(NoticiaModel noticia);
    Task<string> Delete(int id);
}
