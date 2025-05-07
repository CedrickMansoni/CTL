using ctl.share.PWD_App;
using ctl.share.SMS_App;
using ctl.webapi.Data;
using ctl.webapi.Models;
using ctl.webapi.Repository.Banco;
using ctl.webapi.Repository.Campo;
using ctl.webapi.Repository.Marcacao;
using ctl.webapi.Repository.Noticia;
using ctl.webapi.Repository.Usuario;
using ctl.webapi.SalvarArquivos;
using ctl.webapi.Service.Banco;
using ctl.webapi.Service.Campo;
using ctl.webapi.Service.Marcacao;
using ctl.webapi.Service.Noticia;
using ctl.webapi.Service.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

string conexao = builder.Configuration.GetConnectionString("LocalConnection")!;
builder.Services.AddDbContext<AppDataContext>(options => options.UseNpgsql(conexao));

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();

builder.Services.AddTransient<ICampoRepository, CampoRepository>();
builder.Services.AddTransient<ICampoService, CampoService>();

builder.Services.AddTransient<IMarcacaoRepository, MarcacaoRepository>();
builder.Services.AddTransient<IMarcacaoService, MarcacaoService>();

builder.Services.AddTransient<IBancoRepository, BancoRepository>();
builder.Services.AddTransient<IBancoService, BancoService>();

builder.Services.AddTransient<IContaRepository, ContaRepository>();
builder.Services.AddTransient<IContaService, ContaService>();

builder.Services.AddTransient<INoticiaRepository, NoticiaRepository>();
builder.Services.AddTransient<INoticiaService, NoticiaService>();

builder.Services.AddTransient<IHash_PWD, Hash_PWD>();
builder.Services.AddTransient<ISMS_enviar, SMS_enviar>();

builder.Services.AddScoped<IArquivoService, ArquivoService>();

// Adiciona CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyHeader()  // Permite qualquer cabeçalho
              .AllowAnyMethod(); // Permite qualquer método HTTP
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "CTL_APP"));
    app.MapScalarApiReference();
}

string storagePath = app.Environment.IsDevelopment() ?
"/Users/cedrickmansoni/Storage/CTL" :
"/home/GSA_PROJECT/Storage/CTL";

// Configurar middleware para servir arquivos de um diretório externo
app.UseStaticFiles(new StaticFileOptions
{
    //--- 
    FileProvider = new PhysicalFileProvider(storagePath),
    RequestPath = "/images", // Caminho acessível via URL
    ServeUnknownFileTypes = false, // Não servir tipos desconhecidos
    DefaultContentType = "image/jpeg", // Conteúdo padrão caso a extensão não seja reconhecida
    OnPrepareResponse = ctx =>
    {
        // Bloquear acesso a arquivos sem extensões de imagem
        if (!ctx.File.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) &&
            !ctx.File.Name.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
    }
});

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
