using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilmsRazor.Models;

namespace TopFilmsRazor.Pages
{
    public class EditModel(FilmsContext context, IWebHostEnvironment appEnvironment) : PageModel
    {
        private readonly FilmsContext _context = context;

        [BindProperty]
        public Film Film { get; set; } = default!;

        [BindProperty]
        public IFormFile? posterFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || context.Films == null) return NotFound();

            var film = await _context.Films.FindAsync(id);

            if (film == null) return NotFound();
            Film = film;

            return Page();
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id != Film.Id) 
                return NotFound();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldFilm = await _context.Films.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            if (oldFilm is null) return NotFound();

            if (posterFile is not null && posterFile.Length > 0)
            {
                var fileName = Path.GetFileName(posterFile.FileName);
                var relativePath = $"/posters/{fileName}";
                var absolutePath = Path.Combine(appEnvironment.WebRootPath, "posters", fileName);

                await using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    await posterFile.CopyToAsync(fileStream);
                }
                Film.Poster = relativePath;
            }
            else
            {
                Film.Poster = oldFilm.Poster;
            }
            try
            {
                _context.Update(Film);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!FilmExists(Film.Id)) return NotFound();
                throw;
            }
            return RedirectToPage("./Index");
        }

        private bool FilmExists(int id) =>
            (context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
    }

}

