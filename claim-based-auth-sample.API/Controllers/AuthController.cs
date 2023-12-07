using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using claim_based_auth_sample.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace claim_based_auth_sample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // users are cretaed with default claims (email, name). These claims doesn't need to be stored in db.
        // claims like roles need to be stored ni db
        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDTO>> SignUp(UserCredentialsDTO credentials)
        {
            var newUser = new IdentityUser() { UserName = credentials.Email, Email = credentials.Email };
            var identityRes = await _userManager.CreateAsync(newUser, credentials.Password);

            if(identityRes.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest(identityRes.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> LogIn(UserCredentialsDTO credentials)
        {
            var identityRes = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, isPersistent: false, lockoutOnFailure: false);

            if(identityRes.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest("Incorrect login");
            }
        }
        
        [HttpPost("token/refresh")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthResponseDTO>> RefreshToken()
        {
            var userEmailclaim = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Email).FirstOrDefault();
            var userEmail = userEmailclaim.Value;
            var userCredentials = new UserCredentialsDTO()
            {
                Email = userEmail,
            };

            return await BuildToken(userCredentials);
        }

        [HttpPost("grant/admin")]
        public async Task<ActionResult> GrantAdmin(GrantAdminAuthorizationDTO grantAdminDTO)
        {
            var user = await _userManager.FindByEmailAsync(grantAdminDTO.Email);
            await _userManager.AddClaimAsync(user, new Claim("admin", ""));
            return NoContent();
        }

        [HttpPost("revoke/admin")]
        public async Task<ActionResult> RevokeAdmin(GrantAdminAuthorizationDTO grantAdminDTO)
        {
            var user = await _userManager.FindByEmailAsync(grantAdminDTO.Email);
            await _userManager.RemoveClaimAsync(user, new Claim("admin", ""));
            return NoContent();
        }

        private async Task<AuthResponseDTO> BuildToken(UserCredentialsDTO authCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, authCredentials.Email),
                new Claim(ClaimTypes.Name, "Put name here!")
            };

            var user = await _userManager.FindByEmailAsync(authCredentials.Email);
            var claimsDb = await _userManager.GetClaimsAsync(user);  // get all claims in db for this user
            claims.AddRange(claimsDb);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TemporalSecretKey"]));
            var tokenCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(2);

            var securityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: tokenCredentials
            );

            return new AuthResponseDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration,
            };
        }
    }
}