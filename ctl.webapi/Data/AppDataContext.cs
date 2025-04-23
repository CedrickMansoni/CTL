using System;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Data;

public class AppDataContext(DbContextOptions<AppDataContext> options) : DbContext(options)
{
    public required DbSet<TipoUsuarioModel> TabelaTipo { get; set; }
    public required DbSet<UsuarioModel> TabelaUsuario { get; set; }
    public required DbSet<CampoModel> TabelaCampo { get; set; }
    public required DbSet<MarcacaoModel> TabelaMarcacao { get; set; }
    public required DbSet<NoticiaModel> TabelaNoticia { get; set; }
    public required DbSet<EstadoMarcacaoModel> TabelaEstadoMarcacao { get; set; }
    public required DbSet<BancoModel> TabelaBanco { get; set; }
    public required DbSet<ContaModel> TabelaConta { get; set; }
}
