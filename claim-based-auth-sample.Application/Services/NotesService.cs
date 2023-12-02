using AutoMapper;
using claim_based_auth_sample.Core;
using claim_based_auth_sample.DataAccess;
using Microsoft.Extensions.Configuration;

namespace claim_based_auth_sample.Application;

public class NotesService : INotesService
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ICommonCrudRepository<Note> _noteRepository;

    public NotesService(IMapper mapper, IConfiguration configuration, ICommonCrudRepository<Note> noteRepository)
    {
        _noteRepository = noteRepository;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<NoteDTO> CreateNote(CreateNoteDTO dto, string ownerId)
    {
        var note = _mapper.Map<Note>(dto);
        note.OwnerId = ownerId;
        var createdNote = await _noteRepository.Create(note);
        return _mapper.Map<NoteDTO>(createdNote);
    }

    public async Task<List<NoteDTO>> ListNotes()
    {
        var notes = await _noteRepository.List();
        return _mapper.Map<List<NoteDTO>>(notes);
    }
}
