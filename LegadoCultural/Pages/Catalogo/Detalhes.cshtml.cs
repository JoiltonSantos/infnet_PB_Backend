using LegadoCultural.Models;
using LegadoCultural.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegadoCultural.Pages.Catalogo;

public class DetalhesModel : PageModel
{
    private const int PerfilId = 1;

    private readonly ICatalogoService _catalogoService;
    private readonly IVisualizacaoService _visualizacaoService;

    public DetalhesModel(ICatalogoService catalogoService, IVisualizacaoService visualizacaoService)
    {
        _catalogoService = catalogoService;
        _visualizacaoService = visualizacaoService;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Conteudo? Conteudo { get; private set; }
    public Visualizacao? Progresso { get; private set; }
    public bool EhFilme { get; private set; }
    public Serie? SerieDetalhes { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Conteudo = await _catalogoService.GetByIdAsync(Id);
        if (Conteudo is null) return NotFound();

        Progresso = _visualizacaoService.GetByConteudo(PerfilId, Id);
        EhFilme = Conteudo is Filme;
        if (Conteudo is Serie s) SerieDetalhes = s;

        return Page();
    }

    public IActionResult OnPostConcluir()
    {
        _visualizacaoService.MarcarConcluida(PerfilId, Id);
        return RedirectToPage(new { id = Id });
    }
}
