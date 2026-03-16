using System.Globalization;
using LegadoCultural.Models;
using LegadoCultural.ViewModels;

namespace LegadoCultural.Services;

public class VisualizacaoService : IVisualizacaoService
{
    private readonly ICatalogoService _catalogoService;
    private readonly List<Visualizacao> _visualizacoes = [];
    private readonly string _csvPath;
    private int _nextId = 1;

    public VisualizacaoService(ICatalogoService catalogoService, IWebHostEnvironment env)
    {
        _catalogoService = catalogoService;
        _csvPath = Path.Combine(env.ContentRootPath, "Data", "visualizacoes.csv");
        CarregarCsv();
    }

    public IReadOnlyList<VisualizacaoViewModel> GetContinuarAssistindo(int perfilId)
    {
        return _visualizacoes
            .Where(v => v.PerfilId == perfilId && !v.Concluida)
            .OrderByDescending(v => v.UltimaVisualizacao)
            .Select(CriarViewModel)
            .OfType<VisualizacaoViewModel>()
            .ToList();
    }

    public Visualizacao? GetByConteudo(int perfilId, int conteudoId)
    {
        return _visualizacoes
            .Where(v => v.PerfilId == perfilId && v.ConteudoId == conteudoId)
            .OrderByDescending(v => v.UltimaVisualizacao)
            .FirstOrDefault();
    }

    public void IniciarOuAtualizar(int perfilId, int conteudoId, int segundos, int? episodioId = null)
    {
        var visualizacao = _visualizacoes
            .FirstOrDefault(v => v.PerfilId == perfilId && v.ConteudoId == conteudoId && v.EpisodioId == episodioId);

        if (visualizacao is null)
        {
            visualizacao = new Visualizacao
            {
                Id = _nextId++,
                PerfilId = perfilId,
                ConteudoId = conteudoId,
                EpisodioId = episodioId
            };
            _visualizacoes.Add(visualizacao);
        }

        visualizacao.PontoDePausa = segundos;
        visualizacao.UltimaVisualizacao = DateTime.Now;
        visualizacao.PercentualAssistido = CalcularPercentual(conteudoId, episodioId, segundos);

        if (visualizacao.PercentualAssistido >= 95)
            visualizacao.Concluida = true;

        SalvarCsv();
    }

    public void MarcarConcluida(int perfilId, int conteudoId)
    {
        foreach (var v in _visualizacoes.Where(v => v.PerfilId == perfilId && v.ConteudoId == conteudoId))
        {
            v.Concluida = true;
            v.PontoDePausa = 0;
        }
        SalvarCsv();
    }

    private double CalcularPercentual(int conteudoId, int? episodioId, int segundos)
    {
        var conteudo = _catalogoService.GetByIdAsync(conteudoId).GetAwaiter().GetResult();
        if (conteudo is null) return 0;

        int duracaoSegundos = conteudo switch
        {
            Filme filme => filme.Duracao * 60,
            Serie serie when episodioId.HasValue =>
                (serie.Temporadas
                    .SelectMany(t => t.Episodios)
                    .FirstOrDefault(e => e.Id == episodioId.Value)?.Duracao ?? 1) * 60,
            _ => 1
        };

        if (duracaoSegundos <= 0) return 0;
        return Math.Min(100, Math.Round(segundos * 100.0 / duracaoSegundos, 1));
    }

    private VisualizacaoViewModel? CriarViewModel(Visualizacao v)
    {
        var conteudo = _catalogoService.GetByIdAsync(v.ConteudoId).GetAwaiter().GetResult();
        if (conteudo is null) return null;

        Episodio? episodio = null;
        if (v.EpisodioId.HasValue && conteudo is Serie serie)
        {
            episodio = serie.Temporadas
                .SelectMany(t => t.Episodios)
                .FirstOrDefault(e => e.Id == v.EpisodioId.Value);
        }

        return new VisualizacaoViewModel
        {
            Visualizacao = v,
            Conteudo = conteudo,
            Episodio = episodio
        };
    }

    private void CarregarCsv()
    {
        if (!File.Exists(_csvPath)) return;
        foreach (var line in File.ReadAllLines(_csvPath).Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var f = CsvParser.ParseLine(line);
            var v = new Visualizacao
            {
                Id                  = int.Parse(f[0]),
                PerfilId            = int.Parse(f[1]),
                ConteudoId          = int.Parse(f[2]),
                EpisodioId          = string.IsNullOrEmpty(f[3]) ? null : int.Parse(f[3]),
                PontoDePausa        = int.Parse(f[4]),
                PercentualAssistido = double.Parse(f[5], CultureInfo.InvariantCulture),
                UltimaVisualizacao  = DateTime.Parse(f[6]),
                Concluida           = bool.Parse(f[7])
            };
            _visualizacoes.Add(v);
            _nextId = Math.Max(_nextId, v.Id + 1);
        }
    }

    private void SalvarCsv()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_csvPath)!);
        var linhas = new List<string>
        {
            "Id,PerfilId,ConteudoId,EpisodioId,PontoDePausa,PercentualAssistido,UltimaVisualizacao,Concluida"
        };
        foreach (var v in _visualizacoes)
        {
            linhas.Add(string.Join(",",
                v.Id, v.PerfilId, v.ConteudoId,
                v.EpisodioId?.ToString() ?? "",
                v.PontoDePausa,
                v.PercentualAssistido.ToString(CultureInfo.InvariantCulture),
                v.UltimaVisualizacao.ToString("O"),
                v.Concluida));
        }
        File.WriteAllLines(_csvPath, linhas);
    }
}
