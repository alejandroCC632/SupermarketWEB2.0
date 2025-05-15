using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data;
using SupermarketWEB.Models;

namespace SupermarketWEB.Pages.PayModes
{
    
    public class EditModel : PageModel
    {
        private readonly SupermarketContext _context;

        public EditModel(SupermarketContext context)
        {
            _context = context;
        }
        [BindProperty]
        public PayMode payMode { get; set; } = default!;

        public async Task<IActionResult> OnGetasync(int? id)
        {
            if (id == null || _context.PayModes == null)
            {
                return NotFound();
            }
            var paymode = await _context.PayModes.FirstOrDefaultAsync(m => m.Id == id);
            if (paymode == null)
            {
                return NotFound();
            }
            payMode = paymode;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(payMode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (PayModeExists(payMode.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }
        private bool PayModeExists(int id)
        {
            return (_context.PayModes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
