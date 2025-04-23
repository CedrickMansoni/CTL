using ctl.share.DTO_App.Noticia;
using ctl.webapi.Service.Noticia;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ctl.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController(INoticiaService service) : ControllerBase
    {
        private readonly INoticiaService _service = service;

        [HttpPost, Route("/adicionar/noticia")]
        public async Task<IActionResult> AdicionarNoticia()
        {
            try
            {
                // Verificar se a requisição possui conteúdo multimídia
                if (!Request.HasFormContentType)
                {
                    return BadRequest("O conteúdo deve ser enviado como multipart/form-data");
                }

                var form = await Request.ReadFormAsync();

                var noticia = new Noticia_DTO
                {
                    //IdUsuario = Convert.ToInt32(form["idUsuario"]),
                    IdUsuario = int.TryParse(form["idUsuario"], out int IdUsuario) ? IdUsuario : 0,
                    Titulo = form["titulo"]!,
                    Materia = form["materia"]!,
                    Ficheiro = form.Files.GetFile("imagem")!,
                    DataPublicacao = DateTime.UtcNow,
                };

                var result = await _service.Add(noticia);
                
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("/listar/noticia")]
        public async Task<IActionResult> ListarNoticia()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("/atualizar/noticia/{id}")]
        public async Task<IActionResult> AtualizarNoticia(int id, [FromForm] Noticia_DTO noticia)
        {
            try
            {
                var result = await _service.Update(noticia);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("/deletar/noticia/{id}")]
        public async Task<IActionResult> DeletarNoticia(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                return result.Contains("sucesso") ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
