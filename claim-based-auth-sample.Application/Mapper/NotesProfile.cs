using AutoMapper;
using claim_based_auth_sample.Core;

namespace claim_based_auth_sample.Application;

public class NotesProfile : Profile
{
    public NotesProfile()
    {
        CreateMap<CreateNoteDTO, Note>();
        CreateMap<Note, NoteDTO>();
    }
}
