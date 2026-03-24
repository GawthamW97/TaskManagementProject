using Microsoft.AspNetCore.Identity;

namespace TaskManagementApp.Repository
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
