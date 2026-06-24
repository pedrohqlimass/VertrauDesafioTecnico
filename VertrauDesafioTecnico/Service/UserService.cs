using Microsoft.EntityFrameworkCore;
using VertrauDesafioTecnico.DB;
using VertrauDesafioTecnico.Model;

namespace VertrauDesafioTecnico.Service;

public class UserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserModel?> GetByIdAsync(long id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<UserModel> CreateAsync(UserModel userModel)
    {
        if (userModel.DataNascimento.HasValue)
        {
            if (userModel.DataNascimento.Value > DateTime.UtcNow)
                throw new ArgumentException("Data de nascimento não pode ser maior que a data atual.");
            
            userModel.DataNascimento = DateTime.SpecifyKind(userModel.DataNascimento.Value, DateTimeKind.Utc);
           
        }

        _context.Users.Add(userModel);
        await _context.SaveChangesAsync();
        return userModel;
    }

    public async Task<UserModel?> UpdateAsync(long id, UserModel userModel)
    {
        
        if (userModel.DataNascimento.HasValue)
        {
            if (userModel.DataNascimento.Value > DateTime.UtcNow)
                throw new ArgumentException("Data de nascimento não pode ser maior que a data atual.");
            
            userModel.DataNascimento = DateTime.SpecifyKind(userModel.DataNascimento.Value, DateTimeKind.Utc);
           
        }
        
        var existe = await _context.Users.FindAsync(id);
        if (existe is null) return null;
        
        existe.Nome = userModel.Nome;
        existe.Sobrenome = userModel.Sobrenome;
        existe.Email = userModel.Email;
        existe.Genero = userModel.Genero;
        existe.DataNascimento = userModel.DataNascimento;
        
        await _context.SaveChangesAsync();
        return existe;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existe = await _context.Users.FindAsync(id);
        if(existe is null) return false;
        
        _context.Users.Remove(existe);
        await _context.SaveChangesAsync();
        return true;
    }
}