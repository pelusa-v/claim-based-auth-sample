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

        [HttpPost("token/create")]
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
            var userEmailclaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var userEmail = userEmailclaim.Value;
            var userCredentials = new UserCredentialsDTO()
            {
                Email = userEmail,
            };

            return await BuildToken(userCredentials);
        }

        [HttpPost("token/revoke")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthResponseDTO>> RevokeToken()
        {
            // var userEmailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            // var user = await _userManager.FindByEmailAsync(userEmailClaim.Value);
            // _userManager.RemoveClaimAsync(user, userEmailClaim);
            throw new NotImplementedException("");
        }

        private async Task<AuthResponseDTO> BuildToken(UserCredentialsDTO authCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", authCredentials.Email),
                new Claim("user_name", "Put name here!")
            };

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