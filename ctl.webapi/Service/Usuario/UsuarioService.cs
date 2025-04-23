using System;
using ctl.share.DTO_App.Usuario;
using ctl.share.PWD_App;
using ctl.share.SMS_App;
using ctl.webapi.Models;
using ctl.webapi.Repository.Usuario;

namespace ctl.webapi.Service.Usuario;

public class UsuarioService(IUsuarioRepository repository, IHash_PWD hash_PWD, ISMS_enviar enviar) : IUsuarioService
{
    private readonly IUsuarioRepository _repository = repository;
    private readonly IHash_PWD _hash_PWD = hash_PWD;
    private readonly ISMS_enviar enviar = enviar;

    public async Task<string> AddUsuario(Cadastrar_Usuario_DTO usuario, int tipo)
    {
        var senha = new Random().Next(100000, 999999).ToString();

        var senha_ = _hash_PWD.HashSenha(senha);

        var user = await _repository.AddUsuario(new UsuarioModel
        {
            IdTipo = usuario.IdTipo,
            Nome = usuario.Nome,
            Senha = string.IsNullOrEmpty(usuario.Senha) ? _hash_PWD.HashSenha(senha_) : _hash_PWD.HashSenha(usuario.Senha),
            Telefone = usuario.Telefone,
        });

        if (!user.Contains("sucesso"))
            return user;

        string parametro = tipo != 1 ?
            "Você foi cadastrado(a) como funcionário(a) do Clube de Ténis de Luanda" :
            $"Sua conta foi criada com sucesso na plataforma do Clube de Ténis de Luanda";

        string parametro2 = tipo != 1 ?
            $"Acesse com as credenciais:\n" + $"👤 Usuário: {usuario.Telefone}\n" + $"🔑 Senha: {senha}\n\n" :
            $"Para mais informações, entre em contacto com o nosso suporte através do número: +244 923 123 456\n\n";

        var mensagem = new Mensagem
        {
            PhoneNumber = usuario.Telefone,
            MessageBody = $"Olá, {usuario.Nome}! 🎉\n\n" +
                $"{parametro}.\n\n" +
                $"Caso ainda não tenha o nosso App, pode baixar a partir das lojas Google Play Store ou Apple App Store.\n" +
                $"{parametro2}"
                
                
        };

        var sms = new EnviarMensagem
        {
            Mensagem = mensagem
        };

        await enviar.EnviarSMS(sms);
        return user;
    }

    public async Task<string> DeleteUsuario(int id)
    {
        return await _repository.DeleteUsuario(id);
    }

    public async Task<IEnumerable<Usuario_DTO>> GetAllUsuarios()
    {
        return await _repository.GetAllUsuarios();
    }

    public async Task<Usuario_DTO?> LoginUsuario(Login_Usuario_DTO usuario)
    {
        if (string.IsNullOrEmpty(usuario.Telefone) || string.IsNullOrEmpty(usuario.Senha))
            return new Usuario_DTO
            {
                Nome = "Informe o telefone e a senha",
            };
        var usuarioResponse = await _repository.LoginUsuario(usuario.Telefone, _hash_PWD.HashSenha(usuario.Senha));
        if (usuarioResponse == null)
            return new Usuario_DTO
            {
                Nome = "Credenciais inválidas",
            };
        return usuarioResponse;
    }

    public async Task<string> UpdateUsuario(Editar_Usuario_DTO usuario)
    {
        return await _repository.UpdateUsuario(new UsuarioModel
        {
            Id = usuario.Id,
            IdTipo = usuario.IdTipo,
            Nome = usuario.Nome,
            Senha = _hash_PWD.HashSenha(usuario.SenhaNova),
            Telefone = usuario.Telefone,
        });
    }
}
