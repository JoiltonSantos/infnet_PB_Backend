using LegadoCultural.Models;
using LegadoCultural.Models.Enums;

namespace LegadoCultural.Services;

public interface ICatalogoService
{
    Task<IReadOnlyList<Conteudo>> GetAllAsync();
    Task<Conteudo?> GetByIdAsync(int id);
    Task<IReadOnlyList<Conteudo>> SearchAsync(
        string? termo,
        string? genero,
        int? ano,
        string? idioma,
        Ordenacao ordenacao = Ordenacao.MaisRecentes);
    Task<IReadOnlyList<string>> GetGenerosDisponiveisAsync();
    Task<IReadOnlyList<string>> GetIdiomasDisponiveisAsync();
    Task<IReadOnlyList<int>> GetAnosDisponiveisAsync();
}
