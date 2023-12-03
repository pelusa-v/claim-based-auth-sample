using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using claim_based_auth_sample.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace claim_based_auth_sample.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteDTO>>> ListNotes()
        {
            return await _notesService.ListNotes();
        }

        [HttpPost]
        public async Task<ActionResult<NoteDTO>> CreateNote(CreateNoteDTO dto)
        {
            return await _notesService.CreateNote(dto, "");
        }
    }
}