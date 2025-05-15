using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupermarketWEB.Data; // Asegúrate de que este sea el namespace correcto de tu contexto
using SupermarketWEB.Models;
using System.Threading.Tasks;

namespace SupermarketWEB.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly SupermarketContext _context;

        public CreateModel(SupermarketContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public string CategoryName { get; set; } // Propiedad para el nombre de la categoría

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(CategoryName))
            {
                ModelState.AddModelError("CategoryName", "The category name is required.");
                return Page();
            }

            // Buscar la categoría por el nombre ingresado
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == CategoryName);

            if (category == null)
            {
                ModelState.AddModelError("CategoryName", "The category name does not exist.");
                return Page();
            }

            Product.CategoryId = category.Id; // Asignar el Id de la categoría al producto

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}