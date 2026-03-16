using LegadoCultural.Models;
using LegadoCultural.Models.Enums;
using LegadoCultural.Services;
using LegadoCultural.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegadoCultural.Pages;

public class IndexModel : PageModel
{
    private const int PerfilId = 1;

    private readonly ICatalogoService _catalogoService;
    private readonly IVisualizacaoService _visualizacaoService;

    public IndexModel(ICatalogoService catalogoService, IVisualizacaoService visualizacaoService)
    {
        _catalogoService = catalogoService;
        _visualizacaoService = visualizacaoService;
    }

    public IReadOnlyList<VisualizacaoViewModel> ContinuarAssistindo { get; private set; } = [];
    public IReadOnlyList<Conteudo> Destaques { get; private set; } = [];

    public async Task OnGetAsync()
    {
        ContinuarAssistindo = _visualizacaoService.GetContinuarAssistindo(PerfilId);
        Destaques = (await _catalogoService.SearchAsync(null, null, null, null, Ordenacao.MelhorAvaliacao))
            .Take(6)
            .ToList();
    }
}
