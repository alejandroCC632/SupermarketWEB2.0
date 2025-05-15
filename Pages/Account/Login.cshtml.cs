using SupermarketWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace SupermarketWEB.Pages.Account
{

    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User {  get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult>OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            
            if (User.Email == "correo@gmail.com" &&  User.Password == "12345")
            {
                // Se crea los Claim, datos a almacenar en la Cookie 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email,User.Email),
                };
                // Se asoscia los claims creados a un nombre de una Cookie
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                // Se agrega la identidad creada al Claims Pincipal de la aplicacion 
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                //Se registra exitosamente la autenticacion y se crea la Cookie en el navegador 
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
                return RedirectToPage("/Index");
            }
            return Page();
        
        }
    }
}
