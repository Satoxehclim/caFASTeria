using caFASTeria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.FileIO;


namespace caFASTeria.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly CaFasteriaContext _context;
        private static Cuentum _cuenta = new Cuentum();

        public HomeController( CaFasteriaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            _cuenta.IdCuenta = 0;
            _cuenta.Nombre = "";
            _cuenta.Usuario = "";
            _cuenta.Contrasena = "";
            _cuenta.Telefono = "";
            List<Producto> productosMostrados = _context.Productos.ToList();
            HttpContext.Session.SetString("_productos", JsonSerializer.Serialize(productosMostrados));
            HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(_cuenta));
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
        public IActionResult IniciarSesion(string correo, string contrase)
        {
            string erContra = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!#$%&/?\*\-+]).{8,}$";
            string erCorreo = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if ( Regex.IsMatch(contrase, erContra) && Regex.IsMatch(correo, erCorreo))
            {
                Cuentum nuevaCuenta = new Cuentum();
                nuevaCuenta = _context.Cuenta.FirstOrDefault(b => b.Usuario.Equals(correo) && b.Contrasena.Equals(contrase));
                HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(nuevaCuenta));
                return View("Index");
            }
            else
            {
                ViewBag.ErrorLogin = 1;
                return View("Login");
            }
        }

        public IActionResult Registro(string nombre, string correo, string contrase, string telefono)
        {
            string erTelefono = @"^\d{10}$";
            string erContra = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!#$%&/?\*\-+]).{8,}$";
            string erCorreo = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (Regex.IsMatch(telefono, erTelefono) && Regex.IsMatch(contrase, erContra) && Regex.IsMatch(correo, erCorreo) && nombre.Length > 2)
            {
                
                Cuentum nuevaCuenta = new Cuentum();
                nuevaCuenta.Nombre = nombre;
                nuevaCuenta.Usuario = correo;
                nuevaCuenta.Contrasena = contrase;
                nuevaCuenta.Telefono = telefono;
                _context.Cuenta.Add(nuevaCuenta);
                _context.SaveChanges();
                HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(nuevaCuenta));
                return View("Index");
            }
            else
            {
                ViewBag.ErrorRegistro = 1;
                return View("Register");
            }
        }
        public IActionResult Register()
        {
            return View();
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

        public IActionResult Home()
        {
            return View("Index");
        }
         
        public IActionResult AgregarProducto(string nombre, string descripcion, float costo, IFormFile foto)
        {
            if (foto != null && (Path.GetExtension(foto.FileName).ToLower() != ".jpg" || Path.GetExtension(foto.FileName).ToLower() != ".png") && nombre.Length > 2 && descripcion.Length < 200 && descripcion.Length > 2 && costo > 0)
            {
                string cuentaJson = HttpContext.Session.GetString("_cuenta");
                Cuentum cuenta = JsonSerializer.Deserialize<Cuentum>(cuentaJson);
                string rutaSitio = Directory.GetCurrentDirectory();
                string direccionFoto = Path.Combine(rutaSitio, "Repositorio" ,"FotosDeCuenta" + cuenta.IdCuenta, foto.FileName);
                var pathCarpeta = Path.Combine(rutaSitio, "Repositorio", "FotosDeCuenta" + cuenta.IdCuenta);
                if (!Directory.Exists(pathCarpeta))
                {
                    Directory.CreateDirectory(pathCarpeta);
                }
                if (!System.IO.File.Exists(direccionFoto))
                {
                    using (var stream = new FileStream(direccionFoto, FileMode.Create))
                    {
                        Foto nuevaFoto = new Foto();
                        nuevaFoto.Direcion = direccionFoto;
                        _context.Fotos.Add(nuevaFoto);
                        _context.SaveChanges();
                        foto.CopyTo(stream);
                        Producto nuevoProducto = new Producto();
                        nuevoProducto.Nombre = nombre;
                        nuevoProducto.Descripcion = descripcion;
                        nuevoProducto.Precio = costo;
                        nuevoProducto.Vendedor = cuenta.IdCuenta;
                        nuevoProducto.Foto = nuevaFoto.Idfoto;
                        _context.Productos.Add(nuevoProducto);
                        _context.SaveChanges();
                    }
                }
                List<Producto> productosMostrados = _context.Productos.ToList();
                HttpContext.Session.SetString("_productos", JsonSerializer.Serialize(productosMostrados));
                return View("Index");
            }
            else
            {
                return View("Index");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
