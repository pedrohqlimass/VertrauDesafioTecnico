using Microsoft.EntityFrameworkCore;
using VertrauDesafioTecnico.DB;
using VertrauDesafioTecnico.DTOs;
using VertrauDesafioTecnico.Mappers;

namespace VertrauDesafioTecnico.Service;

public class UserService
{
    private readonly AppDbContext _context;
    
    private readonly ILogger<UserService> _logger;
    
    public UserService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        _logger.LogInformation("Buscando todos os usuários");
        
        var users = await _context.Users.ToListAsync();

        if (users is null) 
        {
            _logger.LogWarning("Nenhum usuário foi encontrado no banco de dados");
            return Enumerable.Empty<UserResponseDto>();
        }
        _logger.LogInformation("Usuários encontrados. Quantidade: {Count}", users.Count());
        
        return users.Select(UserMapper.ToDto);
    }

    public async Task<UserResponseDto?> GetByIdAsync(long id)
    {
        _logger.LogInformation("Buscando usuário pelo id: {Id}", id);
        
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            _logger.LogWarning("Usuário não encontrado. Id: {Id}", id);
            return null;
        }
        _logger.LogInformation("Usuário encontrado com sucesso. Id: {Id}", id);
        
        return UserMapper.ToDto(user);
    }

    public async Task<UserResponseDto> CreateAsync(UserCreateDto createDto)
    {
        if (createDto.DataNascimento.HasValue &&
            createDto.DataNascimento.Value > DateTime.UtcNow)
        {
            _logger.LogWarning(
                "Data de nascimento inválida para usuário. Email: {Email} | Data: {Data}",
                createDto.Email,
                createDto.DataNascimento);
            
            throw new ArgumentException("Data inválida");
        }
        
        var emailExiste = await _context.Users
            .AnyAsync(u => u.Email == createDto.Email);

        if (emailExiste)
        {
            _logger.LogWarning("Tentativa de criar usuário com email já existente: {Email}", createDto.Email);
            throw new ArgumentException("Email já cadastrado");
        }

        try
        {
            var user = UserMapper.ToEntity(createDto);

            if (user.DataNascimento.HasValue)
            {
                user.DataNascimento = DateTime.SpecifyKind(user.DataNascimento.Value, DateTimeKind.Utc);
            }
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Usuário criado com sucesso. Id: {Id}", user.Id);
            
            return UserMapper.ToDto(user);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro de banco ao criar usuário. Email: {Email}", createDto.Email);
            throw;
        }
    }

    public async Task<UserResponseDto?> UpdateAsync(long id, UserCreateDto createDto)
    {
        _logger.LogInformation("Atualizando usuário. Id: {Id}", id);
        
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            _logger.LogWarning("Usuário não encontrado para atualização. Id: {Id}", id);
            return null;
        }

        if (createDto.DataNascimento.HasValue &&
            createDto.DataNascimento.Value > DateTime.UtcNow)
        {
            _logger.LogWarning("Data de nascimento inválida para usuário. Email: {Email} | Data: {Data}",
                createDto.Email,
                createDto.DataNascimento);
            
            throw new ArgumentException("Data inválida");
        }
        
        user.Nome = createDto.Nome;
        user.Sobrenome = createDto.Sobrenome;
        user.Email = createDto.Email;
        user.Genero = createDto.Genero;
        user.DataNascimento = createDto.DataNascimento;
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Usuário atualizado com sucesso. Id: {Id}", id);
        
        return UserMapper.ToDto(user);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        _logger.LogInformation("Iniciando deleção de usuário. Id: {Id}", id);
        
        var existe = await _context.Users.FindAsync(id);
        
        if (existe is null)
        {
            _logger.LogWarning("Usuário não  encontrado. Id: {Id}", id);
            return false;
        }
        
        _context.Users.Remove(existe);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Usuário deletado com sucesso. Id: {Id}", id);
        
        return true;
    }
}