using System;
using ctl.share.DTO_App.Usuario;
using ctl.webapi.Data;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Repository.Usuario;

public class UsuarioRepository(AppDataContext context) : IUsuarioRepository
{
    private readonly AppDataContext _context = context;

    public async Task<bool> AbilitarUsuario(Abilitar_Usuario uisuario)
    {
        try
        {
            var usuario = await _context.TabelaUsuario.FindAsync(uisuario.Id);
            if (usuario == null)
            {
                return false;
            }
            usuario.Estado = uisuario.Estado;
            _context.TabelaUsuario.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> AddUsuario(UsuarioModel usuario)
    {
        try
        {
            var usuarioExistente = await _context.TabelaUsuario
                .FirstOrDefaultAsync(u => u.Telefone == usuario.Telefone);
            if (usuarioExistente != null)
            {
                return "Telefone do usuário já está a ser usado em outra conta!";
            }
            await _context.TabelaUsuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return $"Usuario(a) {usuario.Nome} foi adicionado(a) com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao adicionar usuario: {ex.Message}";
        }
    }

    public async Task<string> DeleteUsuario(int id)
    {
        try
        {
            var usuario = await _context.TabelaUsuario.FindAsync(id);
            if (usuario == null)
            {
                return "Usuário não encontrado!";
            }
            usuario.Estado = "Inactivo";
            await _context.SaveChangesAsync();
            return "Usuario removido com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao remover usuario: {ex.Message}";
        }
    }

    public async Task<IEnumerable<Usuario_DTO>> GetAllUsuarios()
    {
        var query = from u in _context.TabelaUsuario
                    join t in _context.TabelaTipo on u.IdTipo equals t.Id
                    select new Usuario_DTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Telefone = u.Telefone,
                        IdTipo = u.IdTipo,
                        Tipo = t.Descricao,
                        Estado = u.Estado
                    };
        return await query.ToListAsync();
    }

    public async Task<Usuario_DTO?> LoginUsuario(string telefone, string senha)
    {
        var query = from u in _context.TabelaUsuario
                    join t in _context.TabelaTipo on u.IdTipo equals t.Id
                    where u.Telefone == telefone && u.Senha == senha
                    select new Usuario_DTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Telefone = u.Telefone,
                        IdTipo = u.IdTipo,
                        Tipo = t.Descricao,
                        Estado = u.Estado
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<string> UpdateUsuario(UsuarioModel usuario)
    {
        try
        {
            var usuarioExistente = await _context.TabelaUsuario.FindAsync(usuario.Id);
            if (usuarioExistente == null)
            {
                return "Usuário não encontrado!";
            }
            usuarioExistente.IdTipo = usuario.IdTipo;
            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Telefone = usuario.Telefone;
            usuarioExistente.Senha = string.IsNullOrEmpty(usuario.Senha) ? usuarioExistente.Senha : usuario.Senha;

            await _context.SaveChangesAsync();

            return "Usuario actualizado com sucesso!";
        }
        catch (Exception ex)
        {
            return $"Erro ao atualizar usuario: {ex.Message}";
        }
    }
}
