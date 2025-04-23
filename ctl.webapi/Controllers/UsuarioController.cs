using ctl.share.DTO_App.Usuario;
using ctl.webapi.Service.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ctl.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioService service) : ControllerBase
    {
        private readonly IUsuarioService _service = service;

        [HttpPost, Route("/cadastrar/funcionario")]
        public async Task<IActionResult> Cadastrar([FromBody] Cadastrar_Usuario_DTO funcionario)
        {
            try
            {
                var response = await _service.AddUsuario(funcionario, 0);
                return response.Contains("sucesso") ? StatusCode(201, response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("/cadastrar/cliente")]
        public async Task<IActionResult> CadastrarCliente([FromBody] Cadastrar_Usuario_DTO cliente)
        {
            try
            {
                var response = await _service.AddUsuario(cliente, 1);
                return response.Contains("sucesso") ? StatusCode(201, response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("/listar/funcionarios")]
        public async Task<IActionResult> ListarFuncionarios()
        {
            try
            {
                var response = await _service.GetAllUsuarios();
                var funcionarios = response.Where(x => x.IdTipo == 2 || x.IdTipo == 3).ToList();
                return Ok(funcionarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("/atualizar/usuario")]
        public async Task<IActionResult> AtualizarFuncionario([FromBody] Editar_Usuario_DTO usuario)
        {
            try
            {
                var response = await _service.UpdateUsuario(usuario);
                return response.Contains("sucesso") ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("/deletar/usuario/{id}")]
        public async Task<IActionResult> DeletarFuncionario(int id)
        {
            try
            {
                var response = await _service.DeleteUsuario(id);
                return response.Contains("sucesso") ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("/login/usuario")]
        public async Task<IActionResult> Login([FromBody] Login_Usuario_DTO usuario)
        {
            try
            {
                var response = await _service.LoginUsuario(usuario);
                if (response!.IdTipo == 0)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
