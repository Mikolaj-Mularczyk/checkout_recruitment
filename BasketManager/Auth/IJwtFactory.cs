using System.Security.Claims;
using System.Threading.Tasks;

namespace BasketManager.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, Claim[] claims);

        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}