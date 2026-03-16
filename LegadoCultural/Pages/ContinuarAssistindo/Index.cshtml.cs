using LegadoCultural.Services;
using LegadoCultural.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegadoCultural.Pages.ContinuarAssistindo;

public class IndexModel : PageModel
{
    private const int PerfilId = 1;

    private readonly IVisualizacaoService _visualizacaoService;

    public IndexModel(IVisualizacaoService visualizacaoService)
    {
        _visualizacaoService = visualizacaoService;
    }

    public IReadOnlyList<VisualizacaoViewModel> Items { get; private set; } = [];

    public void OnGet()
    {
        Items = _visualizacaoService.GetContinuarAssistindo(PerfilId);
    }

    public IActionResult OnPostRemover(int conteudoId)
    {
        _visualizacaoService.MarcarConcluida(PerfilId, conteudoId);
        return RedirectToPage();
    }
}
