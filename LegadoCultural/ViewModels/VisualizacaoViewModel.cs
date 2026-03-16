using LegadoCultural.Models;

namespace LegadoCultural.ViewModels;

public class VisualizacaoViewModel
{
    public Visualizacao Visualizacao { get; set; } = null!;
    public Conteudo Conteudo { get; set; } = null!;
    public Episodio? Episodio { get; set; }
}
