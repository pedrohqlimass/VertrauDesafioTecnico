using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> CreateUser([FromBody] UserModel user)
    {
        try
        {
            var novoUser = await _userService.CreateAsync(user);
            return StatusCode(201, $"Usuário com o id: {novoUser.Id} foi registrado com sucesso");
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
        if (!users.Any()) return NoContent();
        return Ok(users);
    }

    [HttpGet("getuserinfo/{id}")]
    public async Task<IActionResult> GetUserInfoById(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user != null) return Ok(user);
        return NotFound($"Usuário com o id: {id} não existe em nosso sistema");
    }

    [HttpPut("updateuserinfo/{id}")]
    public async Task<IActionResult> UpdateUserInfo(long id, [FromBody] UserModel userAtualizado)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, userAtualizado);
            if (user != null) return Ok($"Usuário com o id: {id} foi atualizado com sucesso");
            return NotFound($"Usuário com o id: {id} não existe em nosso sistema");
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
        if (deletado) return Ok($"Usuário com o id: {id} foi deletado com sucesso.");
        return NotFound($"Usuário com o id: {id} não existe em nosso sistema");
    }
}