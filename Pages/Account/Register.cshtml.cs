using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketWEB.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SupermarketContext _context;

        public RegisterModel(SupermarketContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Verificar que no exista otro usuario con el mismo email
                var exists = _context.Users.Any(u => u.Email == Input.Email);
                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un usuario con este correo.");
                    return Page();
                }

               
                
                

                // Guardar el usuario
                var newUser = new Users
                {

                    Email = Input.Email,
                    Password = Input.Password, // Se guarda en texto plano (inseguro en producción)
                    Salt = ""
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
