using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopFilmsRazor.Models;

namespace TopFilmsRazor.Pages
{
    public class IndexModel(FilmsContext context) : PageModel
    {
        public IList<Film> Films { get; set; } = [];

        public async Task OnGetAsync()
        {
            if (context.Films != null)
            {
                Films = await context.Films.ToListAsync();
            }
        }
    }
}
