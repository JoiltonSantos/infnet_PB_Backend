namespace LegadoCultural.Models;

public class Serie : Conteudo
{
    public int TotalTemporadas { get; set; }
    public List<Temporada> Temporadas { get; set; } = [];
}
