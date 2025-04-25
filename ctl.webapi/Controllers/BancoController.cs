using ctl.share.DTO_App.Banco;
using ctl.webapi.Service.Banco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ctl.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BancoController(IBancoService service) : ControllerBase
    {
        private readonly IBancoService _service = service;

        [HttpPost, Route("/adicionar/banco")]
        public async Task<IActionResult> AdicionarBanco()
        {
            // Verificar se a requisição possui conteúdo multimídia
            if (!Request.HasFormContentType)
            {
                return BadRequest("O conteúdo deve ser enviado como multipart/form-data");
            }

            var form = await Request.ReadFormAsync();

            var banco = new Banco_DTO
            {
                NomeAbreviado = form["nome"]!,
                Logo = form.Files.GetFile("logo")!,
                Estado = form["estado"]!                
            };

            var conta = new Conta_DTO{
                NumeroConta = form["conta"]!,
                IBAN = form["iban"]!
            };
            


            var response = await _service.AddBancoAsync(banco, conta);
            return response.Contains("sucesso") ? Ok(response) : BadRequest(response);
        }

        [HttpGet, Route("/listar/bancos")]
        public async Task<IActionResult> ListarBancos()
        {
            var response = await _service.GetAllBancosAsync();
            return response != null ? Ok(response) : NotFound("Nenhum banco encontrado.");
        }

        [HttpGet, Route("/listar/banco/{id}")]
        public async Task<IActionResult> ListarBancoPorId(int id)
        {
            var response = await _service.GetBancoByIdAsync(id);
            return response != null ? Ok(response) : NotFound("Banco não encontrado.");
        }

        [HttpPut, Route("/editar/banco/{id}")]
        public async Task<IActionResult> EditarBanco(int id)
        {
            // Verificar se a requisição possui conteúdo multimídia
            if (!Request.HasFormContentType)
            {
                return BadRequest("O conteúdo deve ser enviado como multipart/form-data");
            }

            var form = await Request.ReadFormAsync();

            var banco = new Banco_DTO
            {
                Id = id,
                NomeAbreviado = form["nome"]!,
                Logo = form.Files.GetFile("logo")!,
                Estado = form["estado"]!
            };

            var response = await _service.UpdateBancoAsync(banco);
            return response.Contains("sucesso") ? Ok(response) : BadRequest(response);
        }

        [HttpDelete, Route("/remover/banco/{id}")]
        public async Task<IActionResult> RemoverBanco(int id)
        {
            var response = await _service.DeleteBancoAsync(id);
            return response.Contains("sucesso") ? Ok(response) : BadRequest(response);
        }
    }
}
