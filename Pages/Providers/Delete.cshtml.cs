using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.Providers
{
    public class DeleteModel : PageModel
    {
       private readonly SupermarketContext _context;

        public DeleteModel(SupermarketContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Provider Providers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Providers == null)
            {
                return NotFound();
            }
            var Provider = await _context.Providers.FirstOrDefaultAsync(m => m.Id == id);

            if (Provider == null)
            {
                return NotFound();
            }
            else
            {
                Providers = Provider;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Providers == null)
            {
                return NotFound();
            }
            var Provider = await _context.Providers.FindAsync(id);
            if (Provider == null)
            {
                Providers = Provider;
                _context.Providers.Remove(Provider);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");

        }
    }
}
