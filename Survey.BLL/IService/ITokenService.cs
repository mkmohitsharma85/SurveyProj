using Microsoft.AspNetCore.Identity;

namespace Survey.BLL.IService
{
    public interface ITokenService
    {
        Task<string> GenerateToken(IdentityUser identityUser);
    }
}
