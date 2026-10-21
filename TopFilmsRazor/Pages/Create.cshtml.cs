using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopFilmsRazor.Models;


namespace TopFilmsRazor.Pages
{
    public class CreateModel(FilmsContext context, IWebHostEnvironment appEnvironment) : PageModel
    {
        private readonly FilmsContext _context = context;
        public IActionResult OnGet() => Page();

        [BindProperty]
        public Film Film { get; set; } = default!;
        [BindProperty]
        public IFormFile? posterFile { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();


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
            _context.Add(Film);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index"); ;
        }
    }
}
