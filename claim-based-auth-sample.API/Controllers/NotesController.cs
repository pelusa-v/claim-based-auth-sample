using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly UserManager<IdentityUser> _userManager;
        public NotesController(INotesService notesService, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _notesService = notesService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<NoteDTO>>> ListNotes()
        {
            var claims = HttpContext.User.Claims.ToList();
            var emailClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            return await _notesService.ListNotes();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "admin")]
        public async Task<ActionResult<NoteDTO>> CreateNote(CreateNoteDTO dto)
        {
            var userEmailClaim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            var userEmail = userEmailClaim.Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            return await _notesService.CreateNote(dto, user.Id);
        }
    }
}