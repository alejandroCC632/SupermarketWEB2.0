


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
                // Buscar al usuario por correo electr�nico
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == Input.Email);

                if (existingUser != null)
                {
                    // Hash de la contrase�a ingresada para comparar
                  

                    // Verificar si las contrase�as coinciden
                    if (Input.Password == existingUser.Password)
                    {
                        // Las credenciales son v�lidas, crear los Claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, existingUser.Email),
                            // Agrega otros claims seg�n la informaci�n de tu usuario (roles, etc.)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth"); // Usa el mismo esquema
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, new AuthenticationProperties { IsPersistent = Input.RememberMe });

                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login.");
                        return Page();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login.");
                    return Page();
                }
            }

            // Si llegamos aqu�, algo fall�, volvemos a mostrar el formulario
            return Page();
        }

     
    }
}