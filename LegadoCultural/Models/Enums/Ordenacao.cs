using System.ComponentModel.DataAnnotations;

namespace LegadoCultural.Models.Enums;

public enum Ordenacao
{
    [Display(Name = "Mais Recentes")]   
    MaisRecentes,

    [Display(Name = "Mais Antigos")]    
    MaisAntigos,

    [Display(Name = "Título A-Z")]      
    TituloAZ,

    [Display(Name = "Título Z-A")]      
    TituloZA,
    
    [Display(Name = "Melhor Avaliação")]
    MelhorAvaliacao
}
