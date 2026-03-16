namespace LegadoCultural.Models;

public class Temporada
{
    public int Id { get; set; }
    public int SerieId { get; set; }
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public List<Episodio> Episodios { get; set; } = [];
}
