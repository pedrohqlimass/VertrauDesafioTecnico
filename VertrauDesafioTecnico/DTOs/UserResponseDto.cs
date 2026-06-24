using VertrauDesafioTecnico.Model;

namespace VertrauDesafioTecnico.DTOs;

public class UserResponseDto
{
    public long Id { get; set; }
    
    public string Nome { get; set; }
    
    public string Sobrenome { get; set; }
    
    public string Email { get; set; }
    
    public Genero Genero { get; set; }
    
    public DateTime? DataNascimento { get; set; }
}