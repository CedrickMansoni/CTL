using System.Threading.Tasks;
using ctl.share.DTO_App.Campo;
using ctl.webapi.Service.Campo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ctl.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampoController(ICampoService service) : ControllerBase
    {
        private readonly ICampoService _service = service;

        [HttpPost, Route("/cadastrar/campo")]
        public async Task<IActionResult> CadastrarCampo([FromBody] Cadastrar_Campo_DTO campoDto)
        {
            try
            {
                var result = await _service.AddCampo(campoDto);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("/listar/campo")]
        public async Task<IActionResult> ListarCampo()
        {
            try
            {
                var result = await _service.GetAllCampos();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("/editar/campo")]
        public async Task<IActionResult> EditarCampo([FromBody] Editar_Campo_DTO campoDto)
        {
            try
            {
                var result = await _service.UpdateCampo(campoDto);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("/deletar/campo/{id}")]
        public async Task<IActionResult> DeletarCampo(int id)
        {
            try
            {
                var result = await _service.DeleteCampo(id);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut, Route("/desativar/campo")]
        public async Task<IActionResult> DesativarCampo([FromBody] Desativar_Campo_DTO campoDto)
        {
            try
            {
                var result = await _service.AbilitarCampo(campoDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
