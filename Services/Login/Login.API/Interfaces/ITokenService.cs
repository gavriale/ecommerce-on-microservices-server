using Login.API.Entities;

namespace Login.API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
