namespace LegadoCultural.Models;

public class Episodio
{
    public int Id { get; set; }
    public int TemporadaId { get; set; }
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int Duracao { get; set; }
}
