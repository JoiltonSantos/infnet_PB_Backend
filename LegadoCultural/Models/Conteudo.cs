using LegadoCultural.Models.Enums;

namespace LegadoCultural.Models;

public abstract class Conteudo
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Sinopse { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public int Ano { get; set; }
    public Classificacao Classificacao { get; set; }
    public string Idioma { get; set; } = string.Empty;
    public List<string> Elenco { get; set; } = [];
    public string Diretor { get; set; } = string.Empty;
    public string CapaUrl { get; set; } = string.Empty;
    public DateTime DataAdicao { get; set; }
    public double Avaliacao { get; set; }
}
