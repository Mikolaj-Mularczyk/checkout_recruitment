using System;
using System.Security.Claims;
using System.Threading.Tasks;

using BasketManager.Auth;
using BasketManager.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace BasketManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        readonly IJwtFactory jwtFactory;

        private readonly IOptions<JwtIssuerOptions> jwtOptions;

        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions;
        }

        [HttpPost(nameof(Login))]
        public IActionResult Login() // this should have request with credentials
        {
            var userName = "testUser";
            var claims = new[] { new Claim("api_access", "true") };

            var claimsIdentity = jwtFactory.GenerateClaimsIdentity(userName, "1");

            // Serialize and return the response
            var response = new
            {
                id = 1,
                userName,
                isAuthenticated = true,
                claims,
                bearerToken = jwtFactory.GenerateEncodedToken(userName, claimsIdentity, claims).Result,
                expires_in = (int)jwtOptions.Value.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(
                response,
                new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(json);
        }
    }
}
