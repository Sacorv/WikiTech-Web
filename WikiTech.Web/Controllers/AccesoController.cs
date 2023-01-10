//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WikiTech.Entidades;
using WikiTech.Logica;


namespace WikiTech.Web.Controllers
{
    public class AccesoController : Controller
    {

        public IAccesoServicio _IAccesoServicio;
        
        public AccesoController(IAccesoServicio _accesoServicio)
        {
            _IAccesoServicio = _accesoServicio;
        }

        public IActionResult Login(string mensaje)
        {
            ViewBag.mensaje = mensaje;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                
                string tokenString = await _IAccesoServicio.LoginAsync(usuario);
                if (tokenString != "error")
                {
                    // variable de sesion guardada en cookie para usarse configure program.cs
                    HttpContext.Session.SetString("token", tokenString);
                    // para obtener el token usar esto
                    //HttpContext.Session.GetString("token");

                    HttpContext.Session.SetString("email", usuario.email);

                    return Redirect("/Articulo/ListarArticulos");
                }
                else
                {

                    ViewBag.mensaje = "Usuario y/o contraseña invalidos";
                    return Redirect("/Acceso/Login");
                }
            }
            else
            {
                ViewBag.mensaje = "Usuario y/o contraseña invalidos";
                return Redirect("/Acceso/Login");
            }
           
        }

        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return Redirect("/Articulo/ListarArticulos");
        }

        [HttpPost]
        public IActionResult Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _IAccesoServicio.Registrarse(usuario);

                return Redirect("/Articulo/ListarArticulos");
            }
            else
            {
            return View(usuario);

            }
        }
    }
}
