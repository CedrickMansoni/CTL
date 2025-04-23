using System;
using Microsoft.AspNetCore.Http;

namespace ctl.share.DTO_App.Noticia;

public class Noticia_DTO
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Autor { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Materia { get; set; } = string.Empty;

    public DateTime DataPublicacao { get; set; }

    public string Imagem { get; set; } = string.Empty;
    public IFormFile Ficheiro { get; set; } = null!; 
}
