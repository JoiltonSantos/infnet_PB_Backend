using LegadoCultural.Models;
using LegadoCultural.Models.Enums;
using LegadoCultural.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegadoCultural.Pages.Catalogo;

public class IndexModel : PageModel
{
    private readonly ICatalogoService _catalogoService;

    public IndexModel(ICatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    [BindProperty(SupportsGet = true)]
    public string? Termo { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Genero { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Ano { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Idioma { get; set; }

    [BindProperty(SupportsGet = true)]
    public Ordenacao Ordenacao { get; set; } = Ordenacao.MaisRecentes;

    public IReadOnlyList<Conteudo> Resultados { get; private set; } = [];
    public IReadOnlyList<string> Generos { get; private set; } = [];
    public IReadOnlyList<string> Idiomas { get; private set; } = [];
    public IReadOnlyList<int> Anos { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Resultados = await _catalogoService.SearchAsync(Termo, Genero, Ano, Idioma, Ordenacao);
        Generos = await _catalogoService.GetGenerosDisponiveisAsync();
        Idiomas = await _catalogoService.GetIdiomasDisponiveisAsync();
        Anos = await _catalogoService.GetAnosDisponiveisAsync();
    }
}
