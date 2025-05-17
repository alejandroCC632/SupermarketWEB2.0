


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SupermarketWEB.Data;
using SupermarketWEB.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;

namespace SupermarketWEB.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SupermarketContext _context;

        public LoginModel(SupermarketContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync("MyCookieAuth"); // Usa el mismo esquema que en el registro

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Buscar al usuario por correo electrónico
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == Input.Email);

                if (existingUser != null)
                {
                    // Hash de la contraseña ingresada para comparar
                    string hashedPassword = HashPassword(Input.Password, Convert.FromBase64String(existingUser.Salt));

                    // Verificar si las contraseñas coinciden
                    if (hashedPassword == existingUser.Password)
                    {
                        // Las credenciales son válidas, crear los Claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, existingUser.Email),
                            // Agrega otros claims según la información de tu usuario (roles, etc.)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth"); // Usa el mismo esquema
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = Input.RememberMe });

                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // Si llegamos aquí, algo falló, volvemos a mostrar el formulario
            return Page();
        }

        private string HashPassword(string password, byte[] salt)
        {
            // Usa los mismos parámetros que en el registro
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}