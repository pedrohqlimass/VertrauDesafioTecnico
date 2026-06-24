using Microsoft.EntityFrameworkCore;
using VertrauDesafioTecnico.DB;
using VertrauDesafioTecnico.DTOs;
using VertrauDesafioTecnico.Mappers;
using VertrauDesafioTecnico.Model;

namespace VertrauDesafioTecnico.Service;

public class UserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        
        return users.Select(UserMapper.ToDto);
    }

    public async Task<UserResponseDto?> GetByIdAsync(long id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user is null)
            return null;
        
        return UserMapper.ToDto(user);
    }

    public async Task<UserResponseDto> CreateAsync(UserCreateDto createDto)
    {
        if (createDto.DataNascimento.HasValue && createDto.DataNascimento.Value > DateTime.UtcNow)
            throw new ArgumentException("Data inválida");
        
        var user = UserMapper.ToEntity(createDto);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return UserMapper.ToDto(user);
    }

    public async Task<UserResponseDto?> UpdateAsync(long id, UserCreateDto createDto)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user is null)
            return null;
        
        if (createDto.DataNascimento.HasValue && createDto.DataNascimento.Value > DateTime.UtcNow)
            throw new ArgumentException("Data inválida");
        
        user.Nome = createDto.Nome;
        user.Sobrenome = createDto.Sobrenome;
        user.Email = createDto.Email;
        user.Genero = createDto.Genero;
        user.DataNascimento = createDto.DataNascimento;
        
        await _context.SaveChangesAsync();
        
        return UserMapper.ToDto(user);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existe = await _context.Users.FindAsync(id);
        if(existe is null) 
            return false;
        
        _context.Users.Remove(existe);
        await _context.SaveChangesAsync();
        
        return true;
    }
}