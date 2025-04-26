using System;
using ctl.share.DTO_App.Marcacao;
using ctl.webapi.Data;
using ctl.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ctl.webapi.Repository.Marcacao;

public class MarcacaoRepository(AppDataContext context) : IMarcacaoRepository
{
    private readonly AppDataContext _context = context;

    public async Task<string> AlterarMarcacao(MarcacaoModel marcacao)
    {
        var marcacaoDB = await _context.TabelaMarcacao.FindAsync(marcacao.Id);
        if (marcacaoDB == null) return "Marcação não encontrada";
        marcacaoDB.DataInicio = marcacao.DataInicio;
        marcacaoDB.DataTermino = marcacao.DataTermino;

        await _context.SaveChangesAsync();
        return "Marcação alterada com sucesso";
    }

    public async Task<string> CancelarMarcacao(MarcacaoModel marcacao)
    {
        var marcacaoDB = await _context.TabelaMarcacao.FindAsync(marcacao.Id);
        if (marcacaoDB == null) return "Marcação não encontrada";
        marcacaoDB.Observacao = "Cancelada";

        await _context.SaveChangesAsync();
        return "Marcação cancelada com sucesso";
    }

    public async Task<string> ConfirmarMarcacao(EstadoMarcacaoModel marcacao)
    {
        var marcacaoDB = await _context.TabelaEstadoMarcacao.FirstOrDefaultAsync(m => m.IdFuncionario == marcacao.IdFuncionario && m.IdMarcacao == marcacao.IdMarcacao);
        if (marcacaoDB != null) return "Marcação já confirmada";
        await _context.TabelaEstadoMarcacao.AddAsync(marcacao);
        return await _context.SaveChangesAsync() > 0 ? "Marcação confirmada com sucesso" : "Erro ao confirmar a marcação";
    }

    public async Task<string> FazerMarcacao(MarcacaoModel marcacao)
    {
        var dataInicial = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataInicio.Date), DateTimeKind.Utc);
        var dataFinal = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataTermino.Date), DateTimeKind.Utc);
        var marcacoes = await _context.TabelaMarcacao
            .Where(m => m.DataInicio.Date >= dataInicial && m.DataTermino.Date <= dataFinal)
            .ToListAsync();
        if (marcacoes.Count > 0) return "Já existe uma marcação nesse período";
        await _context.TabelaMarcacao.AddAsync(marcacao);
        await _context.SaveChangesAsync();
        return "Marcação realizada com sucesso. Tens 05 minutos para enviar o comprovativo, caso contrario a marcação será cancelada";
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacao(int idCampo, int skip = 0, int take = 30)
    {
        var dataActual = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.UtcNow.Date), DateTimeKind.Utc);
        var marcacoes = from m in _context.TabelaMarcacao
                        join u in _context.TabelaUsuario on m.IdCliente equals u.Id
                        join c in _context.TabelaCampo on m.IdCampo equals c.Id
                        where c.Id == idCampo && m.DataInicio.Date >= dataActual
                        select new Listar_Marcacao_DTO
                        {
                            Id = m.Id,
                            IdCliente = m.IdCliente,
                            Cliente = u.Nome,
                            IdCampo = m.IdCampo,
                            Campo = c.Nome,
                            DataInicio = m.DataInicio,
                            DataTermino = m.DataTermino,
                            Observacao = m.Observacao
                        };
        return await marcacoes.Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync(default);
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorData(MarcacaoModel marcacao, int skip = 0, int take = 30)
    {        
        var dataInicial = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataInicio.Date), DateTimeKind.Utc);
        var dataFinal = DateTime.SpecifyKind(Convert.ToDateTime(marcacao.DataTermino.Date), DateTimeKind.Utc);
        var marcacoes = from m in _context.TabelaMarcacao
                        join u in _context.TabelaUsuario on m.IdCliente equals u.Id
                        join c in _context.TabelaCampo on m.IdCampo equals c.Id
                        where m.DataInicio.Date >= dataInicial && m.DataTermino.Date <= dataFinal
                        select new Listar_Marcacao_DTO
                        {
                            Id = m.Id,
                            IdCliente = m.IdCliente,
                            Cliente = u.Nome,
                            IdCampo = m.IdCampo,
                            Campo = c.Nome,
                            DataInicio = m.DataInicio,
                            DataTermino = m.DataTermino,
                            Observacao = m.Observacao
                        };
        return await marcacoes.Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync(default);
    }
 
    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorStatus(MarcacaoModel marcacao, int skip = 0, int take = 30)
    {
        var marcacoes = from m in _context.TabelaMarcacao
                        join u in _context.TabelaUsuario on m.IdCliente equals u.Id
                        join c in _context.TabelaCampo on m.IdCampo equals c.Id
                        where m.Observacao == marcacao.Observacao
                        select new Listar_Marcacao_DTO
                        {
                            Id = m.Id,
                            IdCliente = m.IdCliente,
                            Cliente = u.Nome,
                            IdCampo = m.IdCampo,
                            Campo = c.Nome,
                            DataInicio = m.DataInicio,
                            DataTermino = m.DataTermino,
                            Observacao = m.Observacao
                        };
        return await marcacoes.Skip(skip).Take(take).OrderByDescending(x => x.Id).ToListAsync(default);
    }

    public async Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorUsuario(MarcacaoModel marcacao)
    {
        var marcacoes = from m in _context.TabelaMarcacao
                        join u in _context.TabelaUsuario on m.IdCliente equals u.Id
                        join c in _context.TabelaCampo on m.IdCampo equals c.Id
                        where m.IdCliente == marcacao.IdCliente
                        select new Listar_Marcacao_DTO
                        {
                            Id = m.Id,
                            IdCliente = m.IdCliente,
                            Cliente = u.Nome,
                            IdCampo = m.IdCampo,
                            Campo = c.Nome,
                            DataInicio = m.DataInicio,
                            DataTermino = m.DataTermino,
                            Observacao = m.Observacao
                        };
        return await marcacoes.OrderByDescending(x => x.Id).ToListAsync(default);
    }

    public async Task<Dadaos_Marcacao_DTO> ObterDadosMarcacao(int idMarcacao)
    {
        var marcacao = from m in _context.TabelaMarcacao
                       join u in _context.TabelaUsuario on m.IdCliente equals u.Id
                       join c in _context.TabelaCampo on m.IdCampo equals c.Id
                       where m.Id == idMarcacao
                       select new Dadaos_Marcacao_DTO
                       {
                           NomeCliente = u.Nome,
                           NomeCampo = c.Nome,
                           DataMarcacao = m.DataInicio.Date,
                           DataInicio = m.DataInicio,
                           DataTermino = m.DataTermino,
                           Telefone = u.Telefone,
                           StatusMarcacao = m.Observacao,
                       };
        var dadosMarcacao = await marcacao.FirstOrDefaultAsync();        
        return  dadosMarcacao ?? new Dadaos_Marcacao_DTO
        {
            NomeCliente = string.Empty,
            NomeCampo = string.Empty,
            DataMarcacao = DateTime.MinValue,
            DataInicio = DateTime.MinValue,
            DataTermino = DateTime.MinValue,
            Telefone = string.Empty,
            StatusMarcacao = string.Empty
        };
    }

    public async Task<MarcacaoModel?> ObterMarcacaoPorId(int id)
    {
        var marcacao = await _context.TabelaMarcacao.FindAsync(id);
        if (marcacao == null) return null;
        return new MarcacaoModel
        {
            Id = marcacao.Id,
            IdCliente = marcacao.IdCliente,
            IdCampo = marcacao.IdCampo,
            DataInicio = marcacao.DataInicio,
            DataTermino = marcacao.DataTermino,
            Observacao = marcacao.Observacao
        };
    }

    public Task<string> RejeitarMarcacao(EstadoMarcacaoModel marcacao)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SalvarComprovativo(Salvar_Comprovativo_DTO comprovativo)
    {
        var marcacao = await _context.TabelaMarcacao.FirstOrDefaultAsync(m => m.Id == comprovativo.IdMarcacao);
        if (marcacao == null) return "Marcação não encontrada";
        if(comprovativo.CodigoTransacao.Equals(marcacao.CodigoTransacao)) return "Comprovativo já enviado";
        marcacao.Comprovativo = comprovativo.Comprovativo!.FileName;
        marcacao.CodigoTransacao = comprovativo.CodigoTransacao;
        marcacao.Observacao = "Comprovativo enviado";
        await _context.SaveChangesAsync();
        return "Comprovativo enviado com sucesso";
    }
}
