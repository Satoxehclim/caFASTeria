using caFASTeria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace caFASTeria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult IniciarSesion()
        {
            return View("Index");
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View("Index");
        }

        public IActionResult Cafeteria(int idCafeteria)
        {
            return View("Index");
        }

        public IActionResult Mercadito()
        {
            return View("Index");
        }

        public IActionResult VerProductos()
        {
            return View("Index");
        }
        public IActionResult VerPedidos()
        {
            return View();
        }
        
        public IActionResult Exit()
        {
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
