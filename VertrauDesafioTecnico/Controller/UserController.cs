using Microsoft.AspNetCore.Mvc;
using VertrauDesafioTecnico.DTOs;
using VertrauDesafioTecnico.Mappers;
using VertrauDesafioTecnico.Model;
using VertrauDesafioTecnico.Service;

namespace VertrauDesafioTecnico.Controller;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto createDto)
    {
        try
        {
            var response = await _userService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetUserInfoById),
                new { id = response.Id },
                response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getuserinfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var users = await _userService.GetAllAsync();
        
        if (!users.Any())
            return NoContent();
        
        return Ok(users);
    }

    [HttpGet("getuserinfo/{id}")]
    public async Task<IActionResult> GetUserInfoById(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        
        if (user is null) 
            return NotFound($"Usuário com o id: {{id}} não existe em nosso sistema");
        
        return Ok(user);
    }

    [HttpPut("updateuserinfo/{id}")]
    public async Task<IActionResult> UpdateUserInfo(long id, [FromBody] UserCreateDto createDto)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, createDto);
            
            if (user is null) 
                return NotFound($"Usuário com o id: {id} foi atualizado com sucesso");
            
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("deleteuserinfo/{id}")]
    public async Task<IActionResult> DeleteUserInfo(long id)
    {
        var deletado = await _userService.DeleteAsync(id);
        
        if (!deletado) 
            return NotFound($"Usuário com o id: {id} não existe em nosso sistema");
        return Ok($"Usuário com o id: {id} foi deletado com sucesso.");
    }
}