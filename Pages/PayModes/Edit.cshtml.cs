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
        }



    }
}
