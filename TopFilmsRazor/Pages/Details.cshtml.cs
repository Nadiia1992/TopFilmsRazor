using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilmsRazor.Models;

namespace TopFilmsRazor.Pages
{

    public class DetailsModel(FilmsContext context) : PageModel
    {
        private readonly FilmsContext _context = context;

        public Film? Film { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null) return NotFound();

            var film = await _context.Films
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

           if (film is null) return NotFound();

            Film = film;
            return Page();
        }
    }
}