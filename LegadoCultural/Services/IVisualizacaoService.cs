using LegadoCultural.Models;
using LegadoCultural.ViewModels;

namespace LegadoCultural.Services;

public interface IVisualizacaoService
{
    IReadOnlyList<VisualizacaoViewModel> GetContinuarAssistindo(int perfilId);
    Visualizacao? GetByConteudo(int perfilId, int conteudoId);
    void IniciarOuAtualizar(int perfilId, int conteudoId, int segundos, int? episodioId = null);
    void MarcarConcluida(int perfilId, int conteudoId);
}
