using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilmsRazor.Models;


namespace TopFilmsRazor.Pages
{
    public class DeleteModel(FilmsContext context) : PageModel
    {
        [BindProperty]
        public Film Film { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || context.Films == null) return NotFound();

            var film = await context.Films.FirstOrDefaultAsync(m => m.Id == id);

            if (film == null) return NotFound();

            Film = film;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || context.Films == null) return NotFound();

            var film = await context.Films.FindAsync(id);

            if (film != null)
            {
                Film = film;
                context.Films.Remove(Film);
                await context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}