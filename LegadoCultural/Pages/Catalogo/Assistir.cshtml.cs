using LegadoCultural.Models;
using LegadoCultural.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegadoCultural.Pages.Catalogo;

public class AssistirModel : PageModel
{
    private const int PerfilId = 1;

    private readonly ICatalogoService _catalogoService;
    private readonly IVisualizacaoService _visualizacaoService;

    public AssistirModel(ICatalogoService catalogoService, IVisualizacaoService visualizacaoService)
    {
        _catalogoService = catalogoService;
        _visualizacaoService = visualizacaoService;
    }

    public Conteudo Conteudo { get; private set; } = null!;
    public int DuracaoSegundos { get; private set; }
    public int PontoInicial { get; private set; }
    public int? EpisodioId { get; private set; }
    public string Subtitulo { get; private set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, int? episodioId)
    {
        var conteudo = await _catalogoService.GetByIdAsync(id);
        if (conteudo is null) return NotFound();

        Conteudo = conteudo;
        EpisodioId = episodioId;

        if (conteudo is Filme filme)
        {
            DuracaoSegundos = filme.Duracao * 60;
        }
        else if (conteudo is Serie serie && episodioId.HasValue)
        {
            var ep = serie.Temporadas.SelectMany(t => t.Episodios)
                         .FirstOrDefault(e => e.Id == episodioId.Value);
            DuracaoSegundos = (ep?.Duracao ?? 45) * 60;
            Subtitulo = ep is not null ? $"Ep. {ep.Numero} – {ep.Titulo}" : string.Empty;
        }
        else
        {
            DuracaoSegundos = 45 * 60;
        }

        var progresso = _visualizacaoService.GetByConteudo(PerfilId, id);
        PontoInicial = progresso is not null && progresso.EpisodioId == episodioId ? progresso.PontoDePausa : 0;

        return Page();
    }

    public IActionResult OnPostSalvar(int conteudoId, int segundos, int? episodioId)
    {
        _visualizacaoService.IniciarOuAtualizar(PerfilId, conteudoId, segundos, episodioId);
        return new JsonResult(new { ok = true });
    }
}
