namespace claim_based_auth_sample.Application;

public interface INotesService
{
    Task<NoteDTO> CreateNote(CreateNoteDTO dto, string ownerId);
    Task<List<NoteDTO>> ListNotes();
}
