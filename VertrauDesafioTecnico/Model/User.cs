namespace VertrauDesafioTecnico.Entity;

public class User
{
    public int id { get; set; }
    public string nome { get; set; }
    public string sobrenome { get; set; }
    public string email { get; set; }
    public Genero genero { get; set; }
    public DateTime dataNascimento { get; set; }
}