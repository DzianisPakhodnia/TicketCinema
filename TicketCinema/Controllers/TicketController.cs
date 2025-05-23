using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketCinema.Contracts;
using TicketCinema.DataAccess;
using TicketCinema.Models;

namespace TicketCinema.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly NotesDbContext _dbContext;

        public TicketController(NotesDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateNoteRequest request, CancellationToken ct)
        {
            var note = new Note(request.Title, request.Description);

            await _dbContext.Notes.AddAsync(note, ct);
            await _dbContext.SaveChangesAsync(ct);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(GetNotesRequest request, CancellationToken ct)
        {
            var notesQuery = _dbContext.Notes
                .Where(n => !string.IsNullOrWhiteSpace(request.Search) &&
                            n.Title.ToLower().Contains(request.Search.ToLower()));



            Expression<Func<Note,object>> selectorKey = request.SortItem?.ToLower() switch
            {
                "date" => note => note.CreatedAt,
                "title" => note => note.Title,
                _ => note => note.Id,
            };



            notesQuery = request.SortOrder == "desc" 
                ? notesQuery.OrderByDescending(selectorKey) 
                : (IQueryable<Note>)notesQuery.OrderBy(selectorKey);

            var noteDtos = await notesQuery
                .Select(n => new NoteDTO(n.Id, n.Title, n.Description, n.CreatedAt))
                .ToListAsync(ct);

            return Ok(new GetNotesResponse(noteDtos));
        }

        
    }
}
