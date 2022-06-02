using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WallOfNotes.Models;

namespace WallOfNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _context = context;
        }

        // GET: api/Notes[?page=1]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes([FromQuery(Name = "page")] int page = 1)
        {
            // sorts by timestamp desc (most recent)
            // paginates by 20
            return await _context.Notes
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * 20).Take(20)
                .ToListAsync();
        }

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, note);
        }
    }
}
