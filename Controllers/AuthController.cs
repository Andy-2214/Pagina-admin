using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly List<string> _admins = new List<string>
        {
            "herbert@gmail.com",
            "roberto@gmail.com",
            "markxx32@gmail.com"
        };

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!_admins.Contains(model.Email))
                {
                    ViewBag.Error = "No tienes permisos de administrador.";
                    return View(model);
                }

                var apiKey = _config["Firebase:ApiKey"];
                var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";

                var payload = new
                {
                    email = model.Email,
                    password = model.Password,
                    returnSecureToken = true
                };

                using var http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("AdminEmail", model.Email);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Email o contraseña incorrectos.";
            }
            catch
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}