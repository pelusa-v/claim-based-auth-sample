using claim_based_auth_sample.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace claim_based_auth_sample.DataAccess;

public class NoteRepository : ICommonCrudRepository<Note>
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _appDbContext;

    public NoteRepository(IConfiguration configuration, AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _configuration = configuration;    
    }

    public async Task<Note> Create(Note note)
    {
        await _appDbContext.Notes.AddAsync(note);
        await _appDbContext.SaveChangesAsync();
        return note;
    }

    public async Task Delete(Note note)
    {
        _appDbContext.Notes.Remove(note);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Note?> Get(int id)
    {
        var noteFound = await _appDbContext.Notes.Where(n => n.NoteId == id).FirstOrDefaultAsync();
        return noteFound;
    }

    public async Task<IEnumerable<Note>> List()
    {
        return await _appDbContext.Notes.ToListAsync();
    }

    public async Task<Note> Update(Note note)
    {
        _appDbContext.Notes.Update(note);
        await _appDbContext.SaveChangesAsync();
        return note;
    }
}
