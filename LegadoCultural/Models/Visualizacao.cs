namespace LegadoCultural.Models;

public class Visualizacao
{
    public int Id { get; set; }
    public int PerfilId { get; set; }
    public int ConteudoId { get; set; }
    public int PontoDePausa { get; set; } // segundos
    public double PercentualAssistido { get; set; } // 0–100
    public DateTime UltimaVisualizacao { get; set; }
    public bool Concluida { get; set; }
    public int? EpisodioId { get; set; }
}
