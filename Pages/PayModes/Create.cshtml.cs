using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketWEB.Data;
using SupermarketWEB.Models;


namespace SupermarketWEB.Pages.PayModes
{
    public class CreateModel : PageModel
    {
        private readonly SupermarketContext _context;

        public CreateModel (SupermarketContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public PayMode payMode { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid || _context.PayModes == null || payMode == null)
            {
                return Page();
            }
            _context.PayModes.Add(payMode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
