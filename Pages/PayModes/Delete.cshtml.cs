using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.PayModes
{
    public class DeleteModel : PageModel
    {
      private readonly SupermarketContext _context;

        public DeleteModel(SupermarketContext context)
        {
            _context = context;
        }
        [BindProperty]
        public PayMode payMode { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id == null || _context.PayModes == null)
            {
                return NotFound();
            }
            var paymodes = await _context.PayModes.FirstOrDefaultAsync(m => m.Id == id);

            if (paymodes == null)
            {
                return NotFound();
            }
            else
            {
                payMode = paymodes;
            }
            return Page();
        }

        public async Task<IActionResult>OnPostAsync(int? id)
        {
            if (id == null || _context.PayModes == null)
            {
                return NotFound();
            }
            var paymodes = await _context.PayModes.FindAsync(id);
            if (paymodes == null)
            {
                payMode = paymodes;
                _context.PayModes.Remove(paymodes);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
