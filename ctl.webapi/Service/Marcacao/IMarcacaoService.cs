using System;
using ctl.share.DTO_App.Marcacao;

namespace ctl.webapi.Service.Marcacao;

public interface IMarcacaoService
{
    Task<string> FazerMarcacao(Fazer_Marcacao_DTO marcacao);
    Task<string> CancelarMarcacao(Cancelar_Marcacao_DTO marcacao);
    Task<string> AlterarMarcacao(Alterar_Marcacao_DTO marcacao);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacao(int idCampo, DateTime dataMarcacao, int skip = 0, int take = 30);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorData(Listar_Marcacao_DTO marcacao, int skip = 0, int take = 30);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorUsuario(int usuario);
    Task<IEnumerable<Listar_Marcacao_DTO>> ListarMarcacaoPorStatus(Listar_Marcacao_DTO marcacao, int skip = 0, int take = 30);
    Task<string> SalvarComprovativo(Salvar_Comprovativo_DTO comprovativo);
    Task<string> ConfirmarMarcacao(Confirmar_Marcacao_DTO marcacao);

}
