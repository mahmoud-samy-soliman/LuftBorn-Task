using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Takamol.Applicaion.Model;
using Takamol.Applicaion.Models;
using Takamol.Domain.Entities;
using Takamol.web.Models;

namespace Takamol.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Login(LoginDto model, [FromServices] IPasswordHasher<AppUser> passwordHasher)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            user.SecurityStamp = Guid.NewGuid().ToString(); 
            await _userManager.UpdateAsync(user);

            if (user != null)
            {
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        string userId = user.Id;
                        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, userId));

                        var claimsList = await _userManager.GetClaimsAsync(user);
                        return GenerateToken(claimsList);
                    }

                    return Unauthorized();
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return BadRequest(ModelState);
        }


        #region token
        private TokenDto GenerateToken(IList<Claim> claimsList)
        {
            string keyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;

            var keyInBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInBytes);

            var signingCredentials = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256Signature);

            var expiry = DateTime.Now.AddMinutes(60);

            var jwt = new JwtSecurityToken(
                    expires: expiry,
                    claims: claimsList,
                    signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);
            foreach (var claim in claimsList)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            return new TokenDto
            (TokenResult.success,
                 tokenString,
                 expiry
            );
        }
        #endregion

    }
}
