using System.ComponentModel.DataAnnotations;
using VertrauDesafioTecnico.Model;

namespace VertrauDesafioTecnico.DTOs;

public class UserCreateDto
{
    public long Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Sobrenome { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public Genero Genero { get; set; }
    
    public DateTime? DataNascimento { get; set; }
}