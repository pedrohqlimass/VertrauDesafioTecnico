using VertrauDesafioTecnico.DTOs;
using VertrauDesafioTecnico.Model;

namespace VertrauDesafioTecnico.Mappers;

public class UserMapper
{
    public static UserModel ToEntity(UserCreateDto userCreateDto)
    {
        return new UserModel
        {
            Nome = userCreateDto.Nome,
            Sobrenome = userCreateDto.Sobrenome,
            Email = userCreateDto.Email,
            Genero = userCreateDto.Genero,
            DataNascimento = userCreateDto.DataNascimento
        };
    }

    public static UserResponseDto ToDto(UserModel userModel)
    {
        return new UserResponseDto
        {
            Id = userModel.Id,
            Nome = userModel.Nome,
            Sobrenome = userModel.Sobrenome,
            Email = userModel.Email,
            Genero = userModel.Genero,
            DataNascimento = userModel.DataNascimento
        };
    }
}