using System;
using ctl.share.Dominio_App;
using ctl.share.DTO_App.Noticia;
using ctl.webapi.Data;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Repository.Noticia;

public class NoticiaRepository(AppDataContext context) : INoticiaRepository
{
    private readonly AppDataContext _context = context;


    public async Task<IEnumerable<Noticia_DTO>> GetAll()
    {
        var noticia = from n in _context.TabelaNoticia
                      join u in _context.TabelaUsuario on n.IdUsuario equals u.Id
                      orderby n.DataPublicacao descending
                      select new Noticia_DTO
                      {
                          Id = n.Id,
                          IdUsuario = n.IdUsuario,
                          Autor = u.Nome,
                          Titulo = n.Titulo,
                          Materia = n.Materia,
                          DataPublicacao = n.DataPublicacao,
                          Imagem = $"{Dominio.URLApp}images/noticia/{n.Imagem}",
                      };
        return await noticia.ToListAsync();
    }

    public async Task<NoticiaModel?> GetById(int id)
    {
        return await _context.TabelaNoticia.FindAsync(id);
    }

    public async Task<string> Add(NoticiaModel noticia)
    {
        try
        {
            await _context.TabelaNoticia.AddAsync(noticia);
            await _context.SaveChangesAsync();
            return "Noticia adicionada com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar noticia: {ex.Message}";
        }
    }

    public async Task<string> Update(NoticiaModel noticia)
    {
        try
        {
            var existingNoticia = await _context.TabelaNoticia.FindAsync(noticia.Id);

            if (existingNoticia == null) return "Noticia não encontrada!";
            existingNoticia.Titulo = noticia.Titulo;
            existingNoticia.Materia = noticia.Materia;
            existingNoticia.Imagem = noticia.Imagem;

            await _context.SaveChangesAsync();
            return "Noticia atualizada com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao atualizar noticia: {ex.Message}";
        }
    }

    public async Task<string> Delete(int id)
    {
        try
        {
            var noticia = await _context.TabelaNoticia.FindAsync(id);

            if (noticia == null) return "Noticia não encontrada!";
            _context.TabelaNoticia.Remove(noticia);
            await _context.SaveChangesAsync();
            return "Noticia excluída com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao excluir noticia: {ex.Message}";
        }
    }
}
