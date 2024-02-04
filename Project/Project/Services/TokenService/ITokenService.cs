using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Project.Models;

namespace Project.Services.TokenService
{
	public interface ITokenService
	{
		public JwtSecurityToken GetToken(List<Claim> authClaims);

        public string GetRefreshToken();

		public ClaimsPrincipal SetRefreshToken(string token);

		public Task AddRefreshTokenToUser(string id, string token);

		public Task<string> Refresh(string Email_or_Username);
	}
}

