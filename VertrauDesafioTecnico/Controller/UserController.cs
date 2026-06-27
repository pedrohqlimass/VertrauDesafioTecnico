using Microsoft.AspNetCore.Mvc;
using VertrauDesafioTecnico.DTOs;
using VertrauDesafioTecnico.Service;

namespace VertrauDesafioTecnico.Controller;
[ApiController]
[Route("usuarios")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    
    private readonly ILogger<UserController> _logger;
    
    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto createDto)
    {
        _logger.LogInformation("Iniciando criação de usuário. Email: {Email}", createDto.Email);
        
        try
        {
            var response = await _userService.CreateAsync(createDto);
            
            _logger.LogInformation("Usuário criado com sucesso. Id: {UserId}", response.Id);

            return CreatedAtAction(
                nameof(GetUserInfoById),
                new { id = response.Id },
                response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            
            return BadRequest(ex.Message);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetUserInfo()
    {
        _logger.LogInformation("Buscando todos os usuários");
        
        var users = await _userService.GetAllAsync();
        
        if (!users.Any())
        {
            _logger.LogWarning("Nenhum usuário foi encontrado.");
            return NoContent();
        }
        _logger.LogInformation("Usuários encontrados. Quantidade: {Count}", users.Count());
        
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfoById(long id)
    {
        _logger.LogInformation("Buscando usuário pelo id: {Id}", id);
        
        var user = await _userService.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning("Usuário não encontrado. Id: {Id}", id);
            
            return NotFound($"Usuário com o id: {id} não existe em nosso sistema");
        }
        _logger.LogInformation("Usuário encontrado com sucesso. Id: {Id}", id);
        
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserInfo(long id, [FromBody] UserCreateDto createDto)
    {
        _logger.LogInformation("Atualizando usuário. Id: {Id}", id);
        
        try
        {
            var user = await _userService.UpdateAsync(id, createDto);

            if (user is null)
            {
                _logger.LogWarning("Usuário não encontrado para atualização. Id: {Id}", id);
                
                return NotFound($"Usuário com o id: {id} não foi encontrado");
            }
            
            _logger.LogInformation("Usuário atualizado com sucesso. Id: {Id}", id);
            
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar usuário. Id: {Id}", id);
            
            return BadRequest("Dados inválidos para atualização do usuário");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserInfo(long id)
    {
        _logger.LogInformation("Iniciando deleção de usuário. Id: {Id}", id);
        
        var deletado = await _userService.DeleteAsync(id);

        if (!deletado)
        {
            _logger.LogWarning("Usuário não  encontrado. Id: {Id}", id);
            
            return NotFound($"Usuário com o id: {id} não existe em nosso sistema");
        }
        _logger.LogInformation("Usuário deletado com sucesso. Id: {Id}", id);
        
        return Ok($"Usuário com o id: {id} foi deletado com sucesso.");
    }
}