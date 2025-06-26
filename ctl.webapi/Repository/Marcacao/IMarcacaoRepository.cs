using System;
using ctl.share.DTO_App.Marcacao;
using ctl.webapi.Models;

namespace ctl.webapi.Repository.Marcacao;

public interface IMarcacaoRepository
{
    Task<string> FazerMarcacao(MarcacaoModel marcacao);
    Task<MarcacaoModel?> ObterMarcacaoPorId(int id);
    Task<string> CancelarMarcacao(MarcacaoModel marcacao);
    Task<string> AlterarMarcacao(MarcacaoModel marcacao);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacao(int skip = 0, int take = 30);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorData(MarcacaoModel marcacao, int skip = 0, int take = 30);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorUsuario(MarcacaoModel marcacao);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorStatus(MarcacaoModel marcacao, int skip = 0, int take = 30);
    Task<string> SalvarComprovativo(Salvar_Comprovativo_DTO comprovativo);
    Task<string> ConfirmarMarcacao(EstadoMarcacaoModel marcacao);
    Task<string> RejeitarMarcacao(EstadoMarcacaoModel marcacao);
    Task<Dados_Marcacao_DTO> ObterDadosMarcacao(int idMarcacao);
}
