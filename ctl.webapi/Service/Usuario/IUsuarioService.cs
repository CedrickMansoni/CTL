using System;
using ctl.share.DTO_App.Usuario;

namespace ctl.webapi.Service.Usuario;

public interface IUsuarioService
{
    Task<string> AddUsuario(Cadastrar_Usuario_DTO usuario, int tipo = 0);
    Task<string> UpdateUsuario(Editar_Usuario_DTO usuario);
    Task<string> DeleteUsuario(int id);
    Task<IEnumerable<Usuario_DTO>> GetAllUsuarios();
    Task<Usuario_DTO?> LoginUsuario(Login_Usuario_DTO usuario);
}
