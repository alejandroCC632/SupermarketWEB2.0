using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupermarketWEB.Models;
using SupermarketWEB.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
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
        public Users NewUser { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var exists = _context.Users.Any(u => u.Email == NewUser.Email);
            if (exists)
            {
                Message = "Este correo ya está registrado.";
                return Page();
            }

            // Generar salt y hashear
            byte[] saltBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);
            string hashedPassword = HashPassword(NewUser.Password, saltBytes);

            // Guardar usuario con salt
            NewUser.Password = hashedPassword;
            NewUser.Salt = salt;

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }

        private string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
