using VertrauDesafioTecnico.Model;

namespace VertrauDesafioTecnico.Service;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<UserModel?> GetByIdAsync(int id);
    Task<UserModel> CreateAsync(UserModel userModel);
    Task<UserModel?> UpadateAsync(int id, UserModel userModel);
    Task <bool> DeleteAsync(int id);
}