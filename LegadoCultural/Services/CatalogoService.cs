using System.Globalization;
using LegadoCultural.Extensions;
using LegadoCultural.Models;
using LegadoCultural.Models.Enums;

namespace LegadoCultural.Services;

public class CatalogoService : ICatalogoService
{
    private readonly List<Filme> _filmes;

    public CatalogoService(IWebHostEnvironment env)
    {
        var csvPath = Path.Combine(env.ContentRootPath, "Data", "filmes.csv");
        _filmes = LerFilmesCsv(csvPath);
    }

    public Task<IReadOnlyList<Conteudo>> GetAllAsync() =>
        Task.FromResult<IReadOnlyList<Conteudo>>(_filmes);

    public Task<Conteudo?> GetByIdAsync(int id) =>
        Task.FromResult<Conteudo?>(_filmes.FirstOrDefault(f => f.Id == id));

    public Task<IReadOnlyList<Conteudo>> SearchAsync(
        string? termo, string? genero, int? ano, string? idioma, Ordenacao ordenacao = Ordenacao.MaisRecentes) =>
        Task.FromResult(SearchSync(termo, genero, ano, idioma, ordenacao));

    public Task<IReadOnlyList<string>> GetGenerosDisponiveisAsync() =>
        Task.FromResult<IReadOnlyList<string>>(
            _filmes.Select(c => c.Genero).Distinct().Order().ToList());

    public Task<IReadOnlyList<string>> GetIdiomasDisponiveisAsync() =>
        Task.FromResult<IReadOnlyList<string>>(
            _filmes.Select(c => c.Idioma).Distinct().Order().ToList());

    public Task<IReadOnlyList<int>> GetAnosDisponiveisAsync() =>
        Task.FromResult<IReadOnlyList<int>>(
            _filmes.Select(c => c.Ano).Distinct().OrderDescending().ToList());

    private IReadOnlyList<Conteudo> SearchSync(
        string? termo, string? genero, int? ano, string? idioma, Ordenacao ordenacao)
    {
        IEnumerable<Conteudo> query = _filmes;

        if (!string.IsNullOrWhiteSpace(termo))
            query = query.Where(c =>
                c.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                c.Diretor.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                c.Elenco.Any(e => e.Contains(termo, StringComparison.OrdinalIgnoreCase)));

        if (!string.IsNullOrWhiteSpace(genero))
            query = query.Where(c => c.Genero == genero);

        if (ano.HasValue)
            query = query.Where(c => c.Ano == ano.Value);

        if (!string.IsNullOrWhiteSpace(idioma))
            query = query.Where(c => c.Idioma == idioma);

        query = ordenacao switch
        {
            Ordenacao.MaisAntigos     => query.OrderBy(c => c.DataAdicao),
            Ordenacao.TituloAZ        => query.OrderBy(c => c.Titulo),
            Ordenacao.TituloZA        => query.OrderByDescending(c => c.Titulo),
            Ordenacao.MelhorAvaliacao => query.OrderByDescending(c => c.Avaliacao),
            _                         => query.OrderByDescending(c => c.DataAdicao),
        };

        return query.ToList();
    }

    private static List<Filme> LerFilmesCsv(string path)
    {
        var filmes = new List<Filme>();
        foreach (var line in File.ReadAllLines(path).Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var f = CsvParser.ParseLine(line);
            filmes.Add(new Filme
            {
                Id            = int.Parse(f[0]),
                Titulo        = f[1],
                Sinopse       = f[2],
                Genero        = f[3],
                Ano           = int.Parse(f[4]),
                Duracao       = int.Parse(f[5]),
                Classificacao = ClassificacaoExtensions.FromString(f[6]),
                Idioma        = f[7],
                Diretor       = f[8],
                Elenco        = [.. f[9].Split(';')],
                CapaUrl       = f[10],
                Avaliacao     = double.Parse(f[11], CultureInfo.InvariantCulture),
                DataAdicao    = DateTime.Parse(f[12])
            });
        }
        return filmes;
    }
}
