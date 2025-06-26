using System;
using ctl.share.DTO_App.Marcacao;
using ctl.share.SMS_App;
using ctl.webapi.Models;
using ctl.webapi.Repository.Marcacao;
using ctl.webapi.SalvarArquivos;
using ctl.webapi.Service.Campo;
using ctl.webapi.Service.Usuario;

namespace ctl.webapi.Service.Marcacao;

public class MarcacaoService(IMarcacaoRepository repository, IConfiguration configuration, IArquivoService arquivoService, ISMS_enviar enviar) : IMarcacaoService
{
    private readonly IMarcacaoRepository _repository = repository;
    private readonly IConfiguration _configuration = configuration;
    private readonly IArquivoService arquivoService = arquivoService;
    private readonly ISMS_enviar _enviar = enviar;

    public async Task<string> AlterarMarcacao(Alterar_Marcacao_DTO marcacao)
    {
        var result = await _repository.AlterarMarcacao(new Models.MarcacaoModel
        {
            Id = marcacao.IdMarcacao,
            DataInicio = marcacao.DataInicio,
            DataTermino = marcacao.DataTermino,
        });
        return result;
    }

    public async Task<string> CancelarMarcacao(Cancelar_Marcacao_DTO marcacao)
    {
        var result = await _repository.CancelarMarcacao(new Models.MarcacaoModel
        {
            Id = marcacao.IdMarcacao,
            Observacao = marcacao.Observacao,
        });
        return result;
    }

    public async Task<string> ConfirmarMarcacao(Confirmar_Marcacao_DTO marcacao)
    {
        if (marcacao.IdFuncionario == 0)
            return "Funcionario não encontrado";
        if (marcacao.IdMarcacao == 0)
            return "Marcacao não encontrada";

        var result = await _repository.ConfirmarMarcacao(new EstadoMarcacaoModel
        {
            IdFuncionario = marcacao.IdFuncionario,
            IdMarcacao = marcacao.IdMarcacao,
        });
        if (result.Contains("sucesso"))
        {
            var dadosMarcacao = await _repository.ObterDadosMarcacao(marcacao.IdMarcacao);

            var mensagem = new Mensagem
            {
                PhoneNumber = dadosMarcacao.Telefone,
                MessageBody = $"A sua marcação foi confirmada com sucesso.\n Detalhes:\n Nome do Cliente: {dadosMarcacao.NomeCliente},\n Nome do Campo: {dadosMarcacao.NomeCampo},\n Data da Marcação: {dadosMarcacao.DataMarcacao},\n Hora de Início: {dadosMarcacao.DataInicio},\n Hora de Término: {dadosMarcacao.DataTermino},\n Telefone: {dadosMarcacao.Telefone},\n Status da Marcação: {dadosMarcacao.StatusMarcacao}"
            };
            var sms = new EnviarMensagem
            {
                Mensagem = mensagem,
            };
            await _enviar.EnviarSMS(sms);
        }
        return result;
    }

    public async Task<string> FazerMarcacao(Fazer_Marcacao_DTO marcacao)
    {
        var result = await _repository.FazerMarcacao(new Models.MarcacaoModel
        {
            IdCliente = marcacao.IdCliente,
            IdCampo = marcacao.IdCampo,
            DataMarcacao = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataMarcacao), DateTimeKind.Utc),
            DataInicio = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataInicio.AddHours(-1)), DateTimeKind.Utc),
            DataTermino = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataTermino.AddHours(-1)), DateTimeKind.Utc),   
            Observacao = "Pendente",
            Comprovativo = "Sem comprovativo",
        });
        return result;
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacao(int skip = 0, int take = 30)
    {
        return await _repository.ListarMarcacao(skip, take);
        
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorData(Listar_Marcacao_DTO marcacao, int skip = 0, int take = 30)
    {
        var result = await _repository.ListarMarcacaoPorData(new Models.MarcacaoModel
        {
            DataInicio = marcacao.DataInicio,
            DataTermino = marcacao.DataTermino,
        }, skip, take);
        return result.Select(m => new Listar_Marcacao_DTO
        {
            Id = m.Id,
            IdCliente = m.IdCliente,
            IdCampo = m.IdCampo,
            DataInicio = m.DataInicio,
            DataTermino = m.DataTermino,
            Observacao = m.Observacao,
        });
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorStatus(Listar_Marcacao_DTO marcacao, int skip = 0, int take = 30)
    {
        var result = await _repository.ListarMarcacaoPorStatus(new Models.MarcacaoModel
        {
            Observacao = marcacao.Observacao,
        }, skip, take);
        return result.Select(m => new Listar_Marcacao_DTO
        {
            Id = m.Id,
            IdCliente = m.IdCliente,
            IdCampo = m.IdCampo,
            DataInicio = m.DataInicio,
            DataTermino = m.DataTermino,
            Observacao = m.Observacao,
        });
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorUsuario(int usuario)
    {
        var result = await _repository.ListarMarcacaoPorUsuario(new Models.MarcacaoModel
        {
            IdCliente = usuario,
        });
        return result.Select(m => new Listar_Marcacao_DTO
        {
            Id = m.Id,
            IdCliente = m.IdCliente,
            Cliente = m.Cliente,
            IdCampo = m.IdCampo,
            Campo = m.Campo,
            DataMarcacao = m.DataMarcacao,
            DataInicio = m.DataInicio,
            DataTermino = m.DataTermino,
            Comprovativo = m.Comprovativo,
            Observacao = m.Observacao,
            ValorPagamento = m.ValorPagamento,
            EstadoMarcacao = m.EstadoMarcacao
        });
    }

    public async Task<string> SalvarComprovativo(Salvar_Comprovativo_DTO comprovativo)
    {
        string storagePath;
        storagePath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
        _configuration["VPSStoragePath:ProdutionStoragePath"]! :
        _configuration["StoragePath:LocalStoragePath"]!;

        var response = await _repository.SalvarComprovativo(comprovativo);
        if (!response.Contains("sucesso")) return response;

        // Garantir que o diretório existe
        if (!Directory.Exists(storagePath))
        {
            Directory.CreateDirectory(storagePath);
        }

        // Salvar a imagem no caminho configurado
        await arquivoService.SalvarArquivoAsync(comprovativo.Comprovativo!, storagePath, $"{comprovativo.Telefone}");

        // Retornar o caminho do arquivo salvo
        return "Comprovativo guardado com sucesso";
    }
}
