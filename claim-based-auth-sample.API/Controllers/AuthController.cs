using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using claim_based_auth_sample.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace claim_based_auth_sample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("signup")]
        public Task<ActionResult<AuthResponseDTO>> SignUp(AuthRequestDTO credentials)
        {
            throw new NotImplementedException("");
        }

        [HttpPost("token/create")]
        public Task<ActionResult<AuthResponseDTO>> LogIn(AuthRequestDTO credentials)
        {
            throw new NotImplementedException("");
        }
        
        [HttpPost("token/refresh")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ActionResult<AuthResponseDTO>> RefreshToken()
        {
            throw new NotImplementedException("");
        }

        [HttpPost("token/revoke")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<ActionResult<AuthResponseDTO>> RevokeToken()
        {
            throw new NotImplementedException("");
        }
    }
}