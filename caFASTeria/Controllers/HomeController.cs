using caFASTeria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.FileIO;
using Microsoft.EntityFrameworkCore;


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

        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            _cuenta.IdCuenta = 0;
            _cuenta.Nombre = "";
            _cuenta.Usuario = "";
            _cuenta.Contrasena = "";
            _cuenta.Telefono = "";
            List<Producto> productosMostrados = _context.Productos.Include(p => p.VendedorNavigation)
                                                                  .Include(p => p.FotoNavigation)
                                                                  .Skip((page-1)*pageSize)
                                                                  .Take(pageSize)
                                                                  .ToList();
            int totalProductos = _context.Productos.Include(p => p.VendedorNavigation)
                                                   .Include(p => p.FotoNavigation)
                                                   .Count();
            PageViewModel<Producto> model = new PageViewModel<Producto>
            {
                Items = productosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalProductos
            };
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta ==2).ToList();
            ViewBag.cafeterias = cafeterias;
            HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(_cuenta));
            ViewData["Title"] = "Inicio";
            return View("Index", model);
        }

        public IActionResult Inicio(int page = 1, int pageSize = 12)
        {
            List<Producto> productosMostrados = _context.Productos.Include(p => p.VendedorNavigation)
                                                                  .Include(p => p.FotoNavigation)
                                                                  .Skip((page - 1) * pageSize)
                                                                  .Take(pageSize)
                                                                  .ToList();
            int totalProductos = _context.Productos.Include(p => p.VendedorNavigation)
                                                   .Include(p => p.FotoNavigation)
                                                   .Count();
            PageViewModel<Producto> model = new PageViewModel<Producto>
            {
                Items = productosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalProductos
            };
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            ViewBag.cafeterias = cafeterias;
            ViewData["Title"] = "Inicio";
            return View("Index", model);
        }

        public IActionResult QuienesSomos()
        {
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            ViewBag.cafeterias = cafeterias;
            ViewData["Title"] = "¿Quienes Somos?";
            return View();
        }

        public IActionResult Login()
        {
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            ViewBag.cafeterias = cafeterias;
            return View();
        }

        public IActionResult IniciarSesion(string correo, string contrase)
        {
            try
            {
            string erContra = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!#$%&/?\*\-+]).{8,}$";
            string erCorreo = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if ( Regex.IsMatch(contrase, erContra) && Regex.IsMatch(correo, erCorreo))
            {
                Cuentum nuevaCuenta = new Cuentum();
                nuevaCuenta = _context.Cuenta.FirstOrDefault(b => b.Usuario.Equals(correo) && b.Contrasena.Equals(contrase));
                if (nuevaCuenta != null) 
                {
                    HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(nuevaCuenta));
                    return Inicio(1,12);
                }
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorLogin = 1;
                return View("Login");
            }
            else
            {
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorLogin = 1;
                return View("Login");
            }

            }
            catch (Exception ex) 
            {
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorLogin = 1;
                return View("Login");
            }
        }

        public IActionResult Register()
        {
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            ViewBag.cafeterias = cafeterias;
            return View();
        }

        public IActionResult Registro(string nombre, string correo, string contrase, string telefono)
        {
            try
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
                nuevaCuenta.TipoCuenta = 1;
                _context.Cuenta.Add(nuevaCuenta);
                _context.SaveChanges();
                HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(nuevaCuenta));
                return Inicio(1, 12);
            }
            else
            {
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorRegistro = 1;
                return View("Register");
            }
            }
            catch (Exception ex) 
            {
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorRegistro = 1;
                return View("Register");
            }
        }

        public IActionResult Cafeteria(int idCafeteria, int page=1,int pageSize = 12)
        {
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            List<Producto> productosMostrados = _context.Productos.Include(p => p.VendedorNavigation)
                                                                  .Include(p => p.FotoNavigation)
                                                                  .Where(p => p.Vendedor == idCafeteria)
                                                                  .Skip((page - 1) * pageSize)
                                                                  .Take(pageSize)
                                                                  .ToList();
            int totalProductos = _context.Productos.Where(p => p.Vendedor == idCafeteria).Count();
            PageViewModel<Producto> model = new PageViewModel<Producto>
            {
                Items = productosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalProductos
            };
            ViewBag.cafeterias = cafeterias;
            ViewBag.cafeteriaActual = idCafeteria;
            ViewData["Title"] = "Cafeteria";
            return View("Index", model);
        }   

        public IActionResult Mercadito(int page = 1, int pageSize = 12 )
        {
            List<Cuentum> cuentasNormales = _context.Cuenta.Where(c => c.TipoCuenta == 1).ToList();
            List<Producto> productosMostrados = new List<Producto>();
            foreach (Cuentum c in cuentasNormales)
            {
                productosMostrados.AddRange(_context.Productos.Where(p => p.Vendedor == c.IdCuenta)
                                                              .Include(p => p.FotoNavigation)
                                                              .Skip((page - 1) * pageSize)
                                                              .Take(pageSize)
                                                              .ToList());
            }
            List<Producto> productosMostrados2 = new List<Producto>();
            foreach (Cuentum c in cuentasNormales)
            {
                productosMostrados2.AddRange(_context.Productos.Where(p => p.Vendedor == c.IdCuenta)
                                                              .Include(p => p.FotoNavigation)
                                                              .ToList());
            }
            int totalProductos = productosMostrados2.Count();
            PageViewModel<Producto> model = new PageViewModel<Producto>
            {
                Items = productosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalProductos
            };
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            ViewBag.cafeterias = cafeterias;
            ViewData["Title"] = "Mercadito";
            return View("Index", model);
        }

        public IActionResult GetImage(string filename)
        {
            string rutaSitio = Directory.GetCurrentDirectory();
            var path = Path.Combine(rutaSitio, "Repositorio", filename);

            if (!System.IO.File.Exists(path))
            {
                path = Path.Combine(rutaSitio, "wwwroot", "assets", "LogocaFASTeria.jpg");
                var imageError = System.IO.File.OpenRead(path);
                return File(imageError, "image/jpeg");
            }

            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg"); // Ajusta el tipo MIME según sea necesario
        }

        public IActionResult Exit()
        {
            return Index(1,12);
        }

        public IActionResult Home()
        {
            return Inicio(1,12);
        }

        public IActionResult AgregarProducto(string nombre, string descripcion, float costo, IFormFile foto)
        {
            if (foto != null && (Path.GetExtension(foto.FileName).ToLower() != ".jpg" || Path.GetExtension(foto.FileName).ToLower() != ".png") && nombre.Length > 2 && descripcion.Length < 200 && descripcion.Length > 2 && costo > 0)
            {
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                string cuentaJson = HttpContext.Session.GetString("_cuenta");
                Cuentum cuenta = JsonSerializer.Deserialize<Cuentum>(cuentaJson);
                string rutaSitio = Directory.GetCurrentDirectory();
                string direccionFoto = Path.Combine(rutaSitio, "Repositorio" ,nombreArchivo);
                var pathCarpeta = Path.Combine(rutaSitio, "Repositorio");
                if (!Directory.Exists(pathCarpeta))
                {
                    Directory.CreateDirectory(pathCarpeta);
                }
                if (!System.IO.File.Exists(direccionFoto))
                {
                    using (var stream = new FileStream(direccionFoto, FileMode.Create))
                    {
                        Foto nuevaFoto = new Foto();
                        nuevaFoto.Direcion = nombreArchivo;
                        _context.Fotos.Add(nuevaFoto);
                        _context.SaveChanges();
                        foto.CopyTo(stream);
                        Producto nuevoProducto = new Producto();
                        nuevoProducto.Nombre = nombre;
                        nuevoProducto.Descripcion = descripcion;
                        nuevoProducto.Precio = costo;
                        nuevoProducto.Vendedor = cuenta.IdCuenta;
                        nuevoProducto.Foto = nuevaFoto.Idfoto;
                        nuevoProducto.Calificacion = 5;
                        _context.Productos.Add(nuevoProducto);
                        _context.SaveChanges();
                    }
                }
                List<Producto> productosMostrados = _context.Productos.ToList();
                ViewBag.productos =productosMostrados;
                return Inicio(1, 12);
            }
            else
            {
                return Inicio();
            }
        }

        public IActionResult VerProductos(int page=1, int pageSize = 20, int error=0, int editado=0)
        {
            string usuarioJson = HttpContext.Session.GetString("_cuenta");
            var _usuario = JsonSerializer.Deserialize<Cuentum>(usuarioJson);
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            List<Producto> productosMostrados = _context.Productos.Where(p => p.Vendedor == _usuario.IdCuenta)
                                                                  .Include(p => p.VendedorNavigation)
                                                                  .Include(p => p.FotoNavigation)
                                                                  .Skip((page - 1) * pageSize)
                                                                  .Take(pageSize)
                                                                  .ToList();
            int totalProductos = _context.Productos.Where(p => p.Vendedor == _usuario.IdCuenta).Count();
            PageViewModel<Producto> model = new PageViewModel<Producto>
            {
                Items = productosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalProductos
            };
            ViewBag.cafeterias = cafeterias;
            ViewBag.ErrorEditar = error;
            ViewBag.OkEditar = editado;
            ViewData["Title"] = "Cafeteria";
            return View("VerProductos", model);
        }
        
        public IActionResult buscarProducto(string nombreProductoBuscado,int page=1, int pageSize = 20, int error=0, int editado=0)
        {
            string usuarioJson = HttpContext.Session.GetString("_cuenta");
            var _usuario = JsonSerializer.Deserialize<Cuentum>(usuarioJson);
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            List<Producto> productosMostrados = _context.Productos.Where(p => p.Nombre.Contains(nombreProductoBuscado))
                                                                  .Include(p => p.VendedorNavigation)
                                                                  .Include(p => p.FotoNavigation)
                                                                  .Skip((page - 1) * pageSize)
                                                                  .Take(pageSize)
                                                                  .ToList();
            int totalProductos = _context.Productos.Where(p => p.Vendedor == _usuario.IdCuenta).Count();
            PageViewModel<Producto> model = new PageViewModel<Producto>
            {
                Items = productosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalProductos
            };
            ViewBag.cafeterias = cafeterias;
            ViewBag.ErrorEditar = error;
            ViewBag.OkEditar = editado;
            ViewData["Title"] = "Cafeteria";
            return View("Index", model);
        }

        public IActionResult EditarProducto(int idProducto,string nombre, string descripcion, float costo, IFormFile foto)
        {
            try
            {
                Producto p = _context.Productos.Include(f => f.FotoNavigation).Where(p => p.IdProducto == idProducto).FirstOrDefault();
                p.Nombre = nombre;
                p.Descripcion = descripcion;
                p.Precio = costo;
                if (foto == null)
                {
                    _context.SaveChanges();
                    return VerProductos(1, 20,0,1);
                }
                else
                {
                    string rutaSitio = Directory.GetCurrentDirectory();
                    var path = Path.Combine(rutaSitio, "Repositorio",p.FotoNavigation.Direcion);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                    var newpath = Path.Combine(rutaSitio, "Repositorio",nombreArchivo);
                    if (!System.IO.File.Exists(newpath))
                    {
                        using (var stream = new FileStream(newpath, FileMode.Create))
                        {
                            p.FotoNavigation.Direcion = nombreArchivo;
                            foto.CopyTo(stream);
                            _context.SaveChanges();
                        }
                    }
                    return VerProductos(1, 20,0,1);
                }
            }
            catch(Exception ex)
            {
                return VerProductos(1,20,1,0);
            }
        }

        public IActionResult EliminarProducto(int idProducto)
        {
            try
            {
                Producto p = _context.Productos.Find(idProducto);
                Foto f = _context.Fotos.Find(p.Foto);
                string rutaSitio = Directory.GetCurrentDirectory();
                var path = Path.Combine(rutaSitio, "Repositorio", p.FotoNavigation.Direcion);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.Remove(p);
                _context.Remove(f);
                _context.SaveChanges();
                return VerProductos(1, 20, 0, 1);
            }
            catch(Exception ex)
            {

                return VerProductos(1, 20, 1, 0);
            }
        }

        public IActionResult EditarPerfil(int idCuenta,string nombre, string correo, string contrase, string telefono)
        {
            try
            {
            string erTelefono = @"^\d{10}$";
            string erContra = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!#$%&/?\*\-+]).{8,}$";
            string erCorreo = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (Regex.IsMatch(telefono, erTelefono) && Regex.IsMatch(contrase, erContra) && Regex.IsMatch(correo, erCorreo) && nombre.Length > 2)
            {

                Cuentum nuevaCuenta = _context.Cuenta.Find(idCuenta);
                nuevaCuenta.Nombre = nombre;
                nuevaCuenta.Usuario = correo;
                nuevaCuenta.Contrasena = contrase;
                nuevaCuenta.Telefono = telefono;
                nuevaCuenta.TipoCuenta = 1;
                _context.SaveChanges();
                HttpContext.Session.SetString("_cuenta", JsonSerializer.Serialize(nuevaCuenta));
                return Inicio(1, 12);
            }
            else
            {
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorRegistro = 1;
                return Inicio(1, 12);
                }

            }
            catch (Exception ex) {
                List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
                ViewBag.cafeterias = cafeterias;
                ViewBag.ErrorRegistro = 1;
                return Inicio(1, 12);
            }
        }

        public IActionResult Comprar(int idProducto, int idComprador, int cantidad, string PuntoEntrega)
        {
            try
            {
                Pedido p = new Pedido();
                p.Producto = idProducto;
                p.Cantidad = cantidad;
                p.Comprador = idComprador;
                p.Estado = 0;
                p.PuntoDeEntrega = PuntoEntrega;
                _context.Pedidos.Add(p);
                _context.SaveChanges();
                ViewBag.PedidoRealizado = true; 
            }catch(Exception ex)
            {
                ViewBag.PedidoError = true;
            }
            return Inicio(1, 12);
        }

        public IActionResult VerPedidosHechos(int page=1,int pageSize=20)
        {
            string usuarioJson = HttpContext.Session.GetString("_cuenta");
            var _usuario = JsonSerializer.Deserialize<Cuentum>(usuarioJson);
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            List<Pedido> PedidosMostrados = _context.Pedidos.Where(p => p.Comprador == _usuario.IdCuenta)
                                                                  .Include(p => p.ProductoNavigation)
                                                                  .Skip((page - 1) * pageSize)
                                                                  .Take(pageSize)
                                                                  .ToList();
            int totalPedidos = _context.Pedidos.Where(p => p.Comprador == _usuario.IdCuenta).Count();
            PageViewModel<Pedido> model = new PageViewModel<Pedido>
            {
                Items = PedidosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalPedidos
            };
            ViewBag.cafeterias = cafeterias;
            ViewData["Title"] = "Pedidos Realizados";
            return View("VerPedidosHechos",model);
        }

        public IActionResult CalificarPedido(int idProducto, int idPedido, int calificacion)
        {
            Producto p = _context.Productos.Find(idProducto);
            p.Calificacion = (p.Calificacion+calificacion)/2;
            Pedido pe = _context.Pedidos.Find(idPedido);
            pe.Estado = 5;
            _context.SaveChanges();
            return VerPedidosHechos(1,20);
        }

        public IActionResult ComprarDeNuevo(int idPedido)
        {
            Pedido p = _context.Pedidos.Find(idPedido);
            p.Estado = 0;
            _context.SaveChanges();
            return VerPedidosHechos(1,20);
        }

        public IActionResult AceptarPedido(int idPedido)
        {
            Pedido p = _context.Pedidos.Find(idPedido);
            p.Estado = 1;
            _context.SaveChanges();
            return VerPedidosRecibidos(1, 20);
        }

        public IActionResult RechazarPedido(int idPedido)
        {
            Pedido p = _context.Pedidos.Find(idPedido);
            p.Estado = 2;
            _context.SaveChanges();
            return VerPedidosRecibidos(1, 20);
        }

        public IActionResult CancelarPedido(int idPedido)
        {
            Pedido p = _context.Pedidos.Find(idPedido);
            p.Estado = 3;
            _context.SaveChanges();
            return VerPedidosHechos(1,20);
        }

        public IActionResult EntregarPedido(int idPedido)
        {
            Pedido p = _context.Pedidos.Find(idPedido);
            p.Estado = 4;
            _context.SaveChanges();
            return VerPedidosRecibidos(1,20);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
/////////////////////////////////////////////////// falta programar//////////////////////////////////////////////////////

        public IActionResult VerPedidosRecibidos(int page=1,int pageSize=20)
        {
            string usuarioJson = HttpContext.Session.GetString("_cuenta");
            var _usuario = JsonSerializer.Deserialize<Cuentum>(usuarioJson);
            List<Cuentum> cafeterias = _context.Cuenta.Where(c => c.TipoCuenta == 2).ToList();
            List<Producto> productosCuenta = _context.Productos.Where(p => p.Vendedor == _usuario.IdCuenta).ToList();
            List<Pedido> Pedidos = new List<Pedido>();
            foreach (Producto prod in productosCuenta)
            {
                Pedidos.AddRange(_context.Pedidos.Where(p => p.Producto == prod.IdProducto)
                                                 .Include(p => p.ProductoNavigation)
                                                 .Include(p => p.CompradorNavigation)
                                                 .ToList());
            }
            int totalPedidos = Pedidos.Count();
            List<Pedido> PedidosMostrados = Pedidos.Skip((page - 1) * pageSize)
                                                   .Take(pageSize)
                                                   .ToList();
            
            PageViewModel<Pedido> model = new PageViewModel<Pedido>
            {
                Items = PedidosMostrados,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalPedidos
            };
            ViewBag.cafeterias = cafeterias;
            ViewData["Title"] = "Pedidos Recibidos";
            return View("VerPedidosRecibidos",model);
        }



    }
}
