using System.ComponentModel.DataAnnotations;

namespace VertrauDesafioTecnico.Entity;

public class UserModel
{
    public long Id { get; set; }
    
    [Required]
    public string Nome { get; set; }
    
    [Required]
    public string Sobrenome { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public Genero Genero { get; set; }
    
    public DateTime? DataNascimento { get; set; }
}