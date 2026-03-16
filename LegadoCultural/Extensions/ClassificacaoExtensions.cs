using LegadoCultural.Models.Enums;

namespace LegadoCultural.Extensions;

public static class ClassificacaoExtensions
{
    public static string Exibir(this Classificacao classificacao) => classificacao switch
    {
        Classificacao.Livre      => "Livre",
        Classificacao.Dez        => "10",
        Classificacao.Doze       => "12",
        Classificacao.Quatorze   => "14",
        Classificacao.Dezesseis  => "16",
        Classificacao.Dezoito    => "18",
        _                        => classificacao.ToString()
    };

    public static Classificacao FromString(string? valor) => valor switch
    {
        "L" or "Livre" or "livre" => Classificacao.Livre,
        "10"                       => Classificacao.Dez,
        "12"                       => Classificacao.Doze,
        "14"                       => Classificacao.Quatorze,
        "16"                       => Classificacao.Dezesseis,
        "18"                       => Classificacao.Dezoito,
        _                          => Classificacao.Livre
    };
}
