using System.Threading.Tasks;
using ctl.share.DTO_App.Marcacao;
using ctl.webapi.Service.Marcacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ctl.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcacaoController(IMarcacaoService service) : ControllerBase
    {
        private readonly IMarcacaoService _service = service;

        [HttpPost, Route("/fazer/marcacao")]
        public async Task<IActionResult> FazerMarcacao([FromBody] Fazer_Marcacao_DTO marcacao)
        {
            var result = await _service.FazerMarcacao(marcacao);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }

        [HttpGet, Route("/listar/marcacoes")]
        public async Task<IActionResult> ListarMarcacoes([FromQuery] int idCampo, [FromQuery] DateTime dataMarcacao, [FromQuery] int skip = 0, [FromQuery] int take = 30)
        {
            var result = await _service.ListarMarcacao(idCampo, dataMarcacao, skip, take);
            return result.ToList().Count != 0 ? Ok(result) : NotFound("Nenhuma marcação encontrada.");
        }

        [HttpGet, Route("/listar/marcacoes/usuario")]
        public async Task<IActionResult> ListarMarcacoesPorUsuario([FromQuery] int idUsuario, [FromQuery] int skip = 0, [FromQuery] int take = 30)
        {
            var result = await _service.ListarMarcacaoPorUsuario(idUsuario);
            return result != null ? Ok(result) : NotFound("Nenhuma marcação encontrada.");
        }

        [HttpGet, Route("/listar/marcacoes/data")]
        public async Task<IActionResult> ListarMarcacoesPorData([FromQuery] Listar_Marcacao_DTO marcacao, [FromQuery] int skip = 0, [FromQuery] int take = 30)
        {
            var result = await _service.ListarMarcacaoPorData(marcacao, skip, take);
            return result != null ? Ok(result) : NotFound("Nenhuma marcação encontrada.");
        }

        [HttpGet, Route("/listar/marcacoes/status")]
        public async Task<IActionResult> ListarMarcacoesPorStatus(Listar_Marcacao_DTO marcacao, [FromQuery] int skip = 0, [FromQuery] int take = 30)
        {
            var result = await _service.ListarMarcacaoPorStatus(marcacao, skip, take);
            return result != null ? Ok(result) : NotFound("Nenhuma marcação encontrada.");
        }

        [HttpPut, Route("/alterar/marcacao")]
        public async Task<IActionResult> AlterarMarcacao([FromBody] Alterar_Marcacao_DTO marcacao)
        {
            var result = await _service.AlterarMarcacao(marcacao);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }

        [HttpDelete, Route("/cancelar/marcacao")]
        public async Task<IActionResult> CancelarMarcacao([FromBody] Cancelar_Marcacao_DTO marcacao)
        {
            var result = await _service.CancelarMarcacao(marcacao);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }

        [HttpPost, Route("/upload/comprovativo")]
        public async Task<IActionResult> UploadComprovativo()
        {
            // Verificar se a requisição possui conteúdo multimídia
            if (!Request.HasFormContentType)
            {
                return BadRequest("O conteúdo deve ser enviado como multipart/form-data");
            }

            var form = await Request.ReadFormAsync();

            var arquivo_ = new Salvar_Comprovativo_DTO
            {
                Comprovativo = form.Files.GetFile("comprovativo")!,
                IdMarcacao = int.TryParse(form["idMarcacao"], out int idMarcacao) ? idMarcacao : 0,
                Telefone = form["telefone"]!,
            };
            var result = await _service.SalvarComprovativo(arquivo_);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }
        
        [HttpPost, Route("/confirmar/comprovativo")]
        public async Task<IActionResult> ConfirmarComprovativo([FromBody] Confirmar_Marcacao_DTO confirmar)
        {
            var result = await _service.ConfirmarMarcacao(confirmar);
            return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
        }
    }
}
