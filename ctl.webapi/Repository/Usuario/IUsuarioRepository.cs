using System;
using ctl.share.DTO_App.Usuario;
using ctl.webapi.Models;

namespace ctl.webapi.Repository.Usuario;

public interface IUsuarioRepository
{
    Task<string> AddUsuario(UsuarioModel usuario);
    Task<string> UpdateUsuario(UsuarioModel usuario);
    Task<string> DeleteUsuario(int id);
    Task<IEnumerable<Usuario_DTO>> GetAllUsuarios();
    Task<Usuario_DTO?> LoginUsuario(string telefone, string senha);
    Task<bool> AbilitarUsuario(Abilitar_Usuario uisuario);
}
