using ctl.share.DTO_App.Banco;
using ctl.webapi.Service.Banco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ctl.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController(IContaService service) : ControllerBase
    {
        private readonly IContaService _service = service;

        [HttpPost, Route("adicionar/conta")]
        public async Task<IActionResult> AdicionarConta([FromBody] Conta_DTO conta)
        {
            try
            {
                var result = await _service.AddAsync(conta);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("listar/contas")]
        public async Task<IActionResult> ListarContas()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("atualizar/conta")]
        public async Task<IActionResult> AtualizarConta([FromBody] Conta_DTO conta)
        {
            try
            {
                var result = await _service.UpdateAsync(conta);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
