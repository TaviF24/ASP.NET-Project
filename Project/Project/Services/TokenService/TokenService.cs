using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Project.Data;
using Microsoft.EntityFrameworkCore;

namespace Project.Services.TokenService
{
    public class TokenService : ITokenService
    {

        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public TokenService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        public string GetRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return refreshToken;
        }


        public ClaimsPrincipal SetRefreshToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }


        public async Task AddRefreshTokenToUser(string id, string token)
        {
            var user = await _appDbContext.UserTokens.FirstOrDefaultAsync(x => x.UserId == id);
            var refreshToken = GetRefreshToken();

            if(user != null)
                _appDbContext.UserTokens.Remove(user);

            await _appDbContext.UserTokens.AddAsync(new ApplicationUserToken
            {
                UserId = id,
                LoginProvider = refreshToken,
                Name = "RefreshToken",
                Expires = DateTime.UtcNow.AddHours(4).AddSeconds(30),
                Token = token
            });

            await _appDbContext.SaveChangesAsync();
        }




        public async Task<string> Refresh(string Email_or_Username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == Email_or_Username || x.Email == Email_or_Username);
            if (user == null)
                return "1";

            var userToken = _appDbContext.UserTokens.FirstOrDefault(x => x.UserId == user.Id);
            if (userToken == null || userToken.Expires <= DateTime.UtcNow.AddHours(2))
                return "2";

            var principal = SetRefreshToken(userToken.Token);

            _appDbContext.UserTokens.Remove(userToken);
            var newRefreshToken = GetRefreshToken();
            var newToken = new JwtSecurityTokenHandler().WriteToken(GetToken(principal.Claims.ToList()));

            await _appDbContext.UserTokens.AddAsync(new ApplicationUserToken
            {
                UserId = userToken.UserId,
                LoginProvider = newRefreshToken,
                Name = "RefreshToken",
                Expires = DateTime.UtcNow.AddHours(4).AddSeconds(30),
                Token = newToken
            });

            await _appDbContext.SaveChangesAsync();
            return newToken;


        }
    }
}

