using System.ComponentModel.DataAnnotations;

namespace ExoApi.Models;

public class Projeto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
    [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    [StringLength(30, ErrorMessage = "O status deve ter no máximo 30 caracteres.")]
    public string Status { get; set; } = string.Empty;
}