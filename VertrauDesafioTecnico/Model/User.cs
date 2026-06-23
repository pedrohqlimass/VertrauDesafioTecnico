using System.ComponentModel.DataAnnotations;

namespace VertrauDesafioTecnico.Entity;

public class User
{
    public long id { get; set; }
    
    [Required]
    public string nome { get; set; }
    
    [Required]
    public string sobrenome { get; set; }
    
    [Required]
    public string email { get; set; }
    
    [Required]
    public Genero genero { get; set; }
    
    public DateTime? dataNascimento { get; set; }
}