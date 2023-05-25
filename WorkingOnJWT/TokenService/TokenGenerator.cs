using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkingOnJWT.Entities;
using WorkingOnJWT.Options;

namespace WorkingOnJWT.TokenService;

public class TokenGenerator
{
     private readonly JwtOption _jwtOption;

     public TokenGenerator(IOptions<JwtOption> jwtOption)
     {
          _jwtOption = jwtOption.Value;
     }
     public string GetToken(User user )
     {
          var claims = new List<Claim>()
          {
               new Claim(ClaimTypes.NameIdentifier,user.Id.ToString() ),
               new Claim(ClaimTypes.Name, user.FirstName)
          };
          var signingKey = System.Text.Encoding.UTF32.GetBytes(_jwtOption.signingKey);
          var security = new JwtSecurityToken(
               issuer: _jwtOption.ValidIssuer,
               audience: _jwtOption.ValidAudience,
               claims: claims,
               expires: DateTime.Now.AddSeconds(_jwtOption.expiresInSeconds),
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256)
          );
          var token = new JwtSecurityTokenHandler().WriteToken(security);
          return token;
        
     }
}