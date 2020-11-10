using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;
using SistemaNotas1.Models.Usuarios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace SistemaNotas1.Controllers
{
    public class TbUsuariosController : Controller
    {
        private readonly sistema_notasContext _context;
        public List<VerEstudiante> listaestud = new List<VerEstudiante>();
        public List<VerProfesor> listadoc = new List<VerProfesor>();
        public List<VerAdministrador> listaadmon = new List<VerAdministrador>();
        public List<InfoPer> personas = new List<InfoPer>();
        IngresoUsuario objItemViewModel = new IngresoUsuario();
        public List<Perfil> ListaPerfil = new List<Perfil>();
        public TbUsuariosController(sistema_notasContext context)
        {
            _context = context;
        }

        // GET: TbUsuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbUsuario.ToListAsync());
        }

        // GET: TbUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbUsuario = await _context.TbUsuario
                .FirstOrDefaultAsync(m => m.CodUsuario == id);
            if (tbUsuario == null)
            {
                return NotFound();
            }

            return View(tbUsuario);
        }

        // GET: TbUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TbUsuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodUsuario,Usuario,Contraseña,CodRol")] TbUsuario tbUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbUsuario);
        }

        // GET: TbUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbUsuario = await _context.TbUsuario.FindAsync(id);
            if (tbUsuario == null)
            {
                return NotFound();
            }
            return View(tbUsuario);
        }

        // POST: TbUsuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodUsuario,Usuario,Contraseña,CodRol")] TbUsuario tbUsuario)
        {
            if (id != tbUsuario.CodUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbUsuarioExists(tbUsuario.CodUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbUsuario);
        }

        // GET: TbUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbUsuario = await _context.TbUsuario
                .FirstOrDefaultAsync(m => m.CodUsuario == id);
            if (tbUsuario == null)
            {
                return NotFound();
            }

            return View(tbUsuario);
        }

        // POST: TbUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbUsuario = await _context.TbUsuario.FindAsync(id);
            _context.TbUsuario.Remove(tbUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbUsuarioExists(int id)
        {
            return _context.TbUsuario.Any(e => e.CodUsuario == id);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Registrar()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Visualizar()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RegistroA()
        {
            ViewBag.showSuccessAlert = false;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> RegistroA(IngresoUsuario useri, List<IFormFile> Upload)
        {
            int CodID = 0;
            int CodID1 = 0;
            foreach (var item in Upload)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        useri.Imagen = stream.ToArray();
                    }
                }
            }

            var consulta = from pers in _context.TbInformacionPersonal
                           where pers.Cui.Equals(useri.Cui)
                           select new
                           {
                               pers.Cui
                           };

            var consulta1 = from pers in _context.TbUsuario
                            where pers.Usuario.Equals(useri.Usuario)
                            select new
                            {
                                pers.Usuario
                            };

            var consulta2 = from pers in _context.TbInformacionPersonal
                            where pers.CorreoElectronico.Equals(useri.Correo)
                            select new
                            {
                                pers.CorreoElectronico
                            };

            System.Diagnostics.Debug.WriteLine("YA CASI ENTRAMOS");
            if (consulta.Count() > 0 || consulta1.Count() > 0 || consulta2.Count() > 0)
            {
                ViewBag.showSuccessAlert = true;
                System.Diagnostics.Debug.WriteLine("ENTRAMOS");
                foreach (var item in consulta)
                {
                    System.Diagnostics.Debug.WriteLine("Cui repetido:");
                }
                foreach (var item in consulta1)
                {
                    System.Diagnostics.Debug.WriteLine("User repetido:");
                }
                foreach (var item in consulta2)
                {
                    System.Diagnostics.Debug.WriteLine("Correo repetido:");
                }
                return View(useri);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ASIGNESE:");
                TbUsuario user = new TbUsuario();
                user.Usuario = useri.Usuario;
                user.Contraseña = Crypto.Hash(useri.Password);
                user.CodRol = 1;
                _context.Add(user);
                _context.SaveChanges();

                CodID1 = user.CodUsuario;

                TbInformacionPersonal informacionPersonal = new TbInformacionPersonal();
                informacionPersonal.Nombre = useri.Nombre;
                informacionPersonal.Apellido = useri.Apellidos;
                informacionPersonal.CorreoElectronico = useri.Correo;
                informacionPersonal.Cui = useri.Cui;
                informacionPersonal.Direccion = useri.Direccion;
                informacionPersonal.Imagen = useri.Imagen;
                informacionPersonal.Telefono = useri.Telefono;

                _context.TbInformacionPersonal.Add(informacionPersonal);
                _context.SaveChanges();

                CodID = informacionPersonal.CodInformacionPersonal;

                TbAdministracion administracion = new TbAdministracion();
                administracion.CodInformacionPersonal = CodID;
                administracion.CodUsuario = CodID1;
                _context.Add(administracion);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

        }

        [Authorize(Roles = "Administrador, Docente")]
        public ActionResult RegistroD()
        {
            ViewBag.showSuccessAlert = false;
            return View();
        }

        [Authorize(Roles = "Administrador, Docente")]
        [HttpPost]
        public async Task<ActionResult> RegistroD(IngresoUsuario useri, List<IFormFile> Upload)
        {
            int CodID = 0;
            int CodID1 = 0;
            foreach (var item in Upload)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        useri.Imagen = stream.ToArray();
                    }
                }
            }

            var consulta = from pers in _context.TbInformacionPersonal
                           where pers.Cui.Equals(useri.Cui)
                           select new
                           {
                               pers.Cui
                           };

            var consulta1 = from pers in _context.TbUsuario
                            where pers.Usuario.Equals(useri.Usuario)
                            select new
                            {
                                pers.Usuario
                            };

            var consulta2 = from pers in _context.TbInformacionPersonal
                            where pers.CorreoElectronico.Equals(useri.Correo)
                            select new
                            {
                                pers.CorreoElectronico
                            };

            if (consulta.Count() > 0 || consulta1.Count() > 0 || consulta2.Count() > 0)
            {
                ViewBag.showSuccessAlert = true;
                foreach (var item in consulta)
                {
                    System.Diagnostics.Debug.WriteLine("Cui repetido:");
                }
                foreach (var item in consulta1)
                {
                    System.Diagnostics.Debug.WriteLine("User repetido:");
                }
                foreach (var item in consulta2)
                {
                    System.Diagnostics.Debug.WriteLine("Correo repetido:");
                }
                return View(useri);
            }
            else
            {

                TbUsuario user = new TbUsuario();
                user.Usuario = useri.Usuario;
                user.Contraseña = Crypto.Hash(useri.Password);
                user.CodRol = 2;
                _context.Add(user);
                _context.SaveChanges();

                CodID1 = user.CodUsuario;

                TbInformacionPersonal informacionPersonal = new TbInformacionPersonal();
                informacionPersonal.Nombre = useri.Nombre;
                informacionPersonal.Apellido = useri.Apellidos;
                informacionPersonal.CorreoElectronico = useri.Correo;
                informacionPersonal.Cui = useri.Cui;
                informacionPersonal.Direccion = useri.Direccion;
                informacionPersonal.Imagen = useri.Imagen;
                informacionPersonal.Telefono = useri.Telefono;

                _context.TbInformacionPersonal.Add(informacionPersonal);
                _context.SaveChanges();

                CodID = informacionPersonal.CodInformacionPersonal;

                TbDocente administracion = new TbDocente();
                administracion.CodInformacionPersonal = CodID;
                administracion.CodUsuario = CodID1;
                _context.Add(administracion);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "Administrador, Estudiante")]
        public ActionResult RegistroE()
        {
            ViewBag.showSuccessAlert = false;

            objItemViewModel.ListaGrados = (from objCat in _context.TbGrado
                                            join objCat1 in _context.TbNivel on objCat.CodNivel equals objCat1.CodNivel
                                            select new SelectListItem()
                                            {
                                                Text = String.Concat(objCat.Nombre, " ", objCat1.Nombre),
                                                Value = objCat.CodGrado.ToString(),
                                                Selected = true
                                            });

            return View(objItemViewModel);
        }

        [Authorize(Roles = "Administrador, Estudiante")]
        [HttpPost]
        public async Task<ActionResult> RegistroE(IngresoUsuario useri, List<IFormFile> Upload)
        {

            int CodID = 0;
            int CodID1 = 0;
            foreach (var item in Upload)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        useri.Imagen = stream.ToArray();
                    }
                }
            }

            if (useri.Cui.Length != 13)
            {
                ViewBag.showSuccessAlert = true;
                ModelState.AddModelError("Cui", "La longitud del CUI debe ser de 13 dígitos.");
                LlenarEstudiantes();
                return View(objItemViewModel);
            }
            else
            {

                var consulta = from pers in _context.TbInformacionPersonal
                               where pers.Cui.Equals(useri.Cui)
                               select new
                               {
                                   pers.Cui
                               };

                var consulta1 = from pers in _context.TbUsuario
                                where pers.Usuario.Equals(useri.Usuario)
                                select new
                                {
                                    pers.Usuario
                                };

                var consulta2 = from pers in _context.TbInformacionPersonal
                                where pers.CorreoElectronico.Equals(useri.Correo)
                                select new
                                {
                                    pers.CorreoElectronico
                                };

                if (consulta.Count() > 0 || consulta1.Count() > 0 || consulta2.Count() > 0)
                {
                    ViewBag.showSuccessAlert = true;
                    foreach (var item in consulta)
                    {
                        System.Diagnostics.Debug.WriteLine("Cui repetido:");
                    }
                    foreach (var item in consulta1)
                    {
                        System.Diagnostics.Debug.WriteLine("User repetido:");
                    }
                    foreach (var item in consulta2)
                    {
                        System.Diagnostics.Debug.WriteLine("Correo repetido:");
                    }
                    LlenarEstudiantes();
                    return View(objItemViewModel);
                }
                else
                {

                    TbUsuario user = new TbUsuario();
                    user.Usuario = useri.Usuario;
                    user.Contraseña = Crypto.Hash(useri.Password);
                    user.CodRol = 3;
                    _context.Add(user);
                    _context.SaveChanges();

                    CodID1 = user.CodUsuario;

                    TbInformacionPersonal informacionPersonal = new TbInformacionPersonal();
                    informacionPersonal.Nombre = useri.Nombre;
                    informacionPersonal.Apellido = useri.Apellidos;
                    informacionPersonal.CorreoElectronico = useri.Correo;
                    informacionPersonal.Cui = useri.Cui;
                    informacionPersonal.Direccion = useri.Direccion;
                    informacionPersonal.Imagen = useri.Imagen;
                    informacionPersonal.Telefono = useri.Telefono;

                    _context.TbInformacionPersonal.Add(informacionPersonal);
                    _context.SaveChanges();

                    CodID = informacionPersonal.CodInformacionPersonal;

                    int codigoal = useri.CodGrado ?? default(int);
                    TbAlumno alumno = new TbAlumno();
                    alumno.CodInformacionPersonal = CodID;
                    alumno.CodUsuario = CodID1;
                    alumno.CodGrado = codigoal;
                    alumno.Codigo = useri.Codigo;
                    _context.TbAlumno.Add(alumno);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public void LlenarEstudiantes()
        {


            objItemViewModel.ListaGrados = (from objCat in _context.TbGrado
                                            join objCat1 in _context.TbNivel on objCat.CodNivel equals objCat1.CodNivel
                                            select new SelectListItem()
                                            {
                                                Text = String.Concat(objCat.Nombre, " ", objCat1.Nombre),
                                                Value = objCat.CodGrado.ToString(),
                                                Selected = true
                                            });

        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TbUsuario login, string ReturnUrl = "")
        {


            using (sistema_notasContext dc = new sistema_notasContext())
            {
                ClaimsIdentity identity = null;
                bool isAuthenticated = false;
                var ad1 = dc.TbUsuario.FirstOrDefaultAsync();
                var ad = dc.TbUsuario.FirstOrDefault();
                var c = dc.TbUsuario.Where(w => w.Usuario == login.Usuario).FirstOrDefault();
                Console.WriteLine("Usuario:" + c.Usuario);
                Console.WriteLine("Usuario:" + c.CodUsuario);
                var adc = c.CodRol;
                if (c != null)
                {
                    if (string.Compare(Crypto.Hash(login.Contraseña), c.Contraseña) == 0)
                    {

                        if (adc == 1)
                        {
                            var abc = dc.TbAdministracion.Where(x => x.CodUsuario == c.CodUsuario).FirstOrDefault();
                            Console.WriteLine("Codigo de Información Personal:" + abc.CodInformacionPersonal);
                            var abc1 = dc.TbInformacionPersonal.Where(y => y.CodInformacionPersonal == abc.CodInformacionPersonal).FirstOrDefault();

                            var nombres = abc1.Nombre;
                            Console.WriteLine("Nombre Completo:" + nombres);
                            var nombre = abc1.Nombre + " " + abc1.Apellido;

                            Console.WriteLine("Nombre Completo:" + nombre);
                            System.Diagnostics.Debug.WriteLine("nombre" + nombre);


                            var rols = dc.TbRol.Where(r => r.CodRol == c.CodRol).FirstOrDefault();

                            Console.WriteLine("ROL:" + rols.Rol);
                            System.Diagnostics.Debug.WriteLine("ROL:" + rols.Rol);



                            identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, nombre),
                        new Claim(ClaimTypes.Role, rols.Rol),
                        new Claim(ClaimTypes.NameIdentifier, c.CodUsuario+"")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                            isAuthenticated = true;


                            if (isAuthenticated)
                            {
                                //    contador_sesion++;
                                var principal = new ClaimsPrincipal(identity);
                                var loginA = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                return RedirectToAction("Index", "Home");

                            }

                        }
                        else if (adc == 2)
                        {
                            var abc = dc.TbDocente.Where(x => x.CodUsuario == c.CodUsuario).FirstOrDefault();
                            Console.WriteLine("Codigo de Información Personal:" + abc.CodInformacionPersonal);
                            var abc1 = dc.TbInformacionPersonal.Where(y => y.CodInformacionPersonal == abc.CodInformacionPersonal).FirstOrDefault();

                            var nombres = abc1.Nombre;
                            Console.WriteLine("Nombre Completo:" + nombres);
                            var nombre = abc1.Nombre + " " + abc1.Apellido;

                            Console.WriteLine("Nombre Completo:" + nombre);
                            System.Diagnostics.Debug.WriteLine("nombre" + nombre);


                            var rols = dc.TbRol.Where(r => r.CodRol == c.CodRol).FirstOrDefault();

                            Console.WriteLine("ROL:" + rols.Rol);
                            System.Diagnostics.Debug.WriteLine("ROL:" + rols.Rol);



                            identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, nombre),
                        new Claim(ClaimTypes.Role, rols.Rol),
                        new Claim(ClaimTypes.NameIdentifier, c.CodUsuario+"")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                            isAuthenticated = true;


                            if (isAuthenticated)
                            {
                                //    contador_sesion++;
                                var principal = new ClaimsPrincipal(identity);
                                var loginA = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                return RedirectToAction("Index", "Home");

                            }
                        }
                        else
                        {
                            var abc = dc.TbAlumno.Where(x => x.CodUsuario == c.CodUsuario).FirstOrDefault();
                            Console.WriteLine("Codigo de Información Personal:" + abc.CodInformacionPersonal);
                            var abc1 = dc.TbInformacionPersonal.Where(y => y.CodInformacionPersonal == abc.CodInformacionPersonal).FirstOrDefault();

                            var nombres = abc1.Nombre;
                            Console.WriteLine("Nombre Completo:" + nombres);
                            var nombre = abc1.Nombre + " " + abc1.Apellido;

                            Console.WriteLine("Nombre Completo:" + nombre);
                            System.Diagnostics.Debug.WriteLine("nombre" + nombre);


                            var rols = dc.TbRol.Where(r => r.CodRol == c.CodRol).FirstOrDefault();

                            Console.WriteLine("ROL:" + rols.Rol);
                            System.Diagnostics.Debug.WriteLine("ROL:" + rols.Rol);



                            identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, nombre),
                        new Claim(ClaimTypes.Role, rols.Rol),
                        new Claim(ClaimTypes.NameIdentifier, c.CodUsuario+"")
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                            isAuthenticated = true;


                            if (isAuthenticated)
                            {
                                //    contador_sesion++;
                                var principal = new ClaimsPrincipal(identity);
                                var loginA = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                return RedirectToAction("Index", "Home");

                            }
                        }



                    }
                    else
                    {
                        RedirectToAction("Login");
                    }

                }
                else
                {
                    RedirectToAction("Login");
                }

            }

            return View();
        }


        [HttpGet]
        public ActionResult Logout()
        {
            var loginA = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // contador_sesion++;
            //    contador_inicio_sesion++;
            //Console.WriteLine("contador index:"+TbEstudiantexcursoesController.contador3);
            //  TbEstudiantexcursoesController.contador3 = 0;
            //TbEstudiantexcursoesController.asCurso.Clear();
            return RedirectToAction("Login");

        }

        //Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(string j)
        {
            var loginA = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult getImage(int id)
        {
            System.Diagnostics.Debug.WriteLine("CODIGO" + id);
            using (var context = new sistema_notasContext())
            {
                var imagen = (from producto in context.TbInformacionPersonal
                              where producto.CodInformacionPersonal == id
                              select producto.Imagen).FirstOrDefault();

                if (imagen == null)
                {
                    return File("~/Imagenes/imangennoencontrada.png", "Imagenes/jpeg");
                }
                else
                {

                    return File(imagen, "Imagenes/jpeg");
                }

            }

            /*    TbProducto prodcto = _context.TbProducto.Find(id);
            byte[] byteImage = prodcto.Imagen;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);
            */
        }

        public IActionResult getGrado(int id)
        {
            System.Diagnostics.Debug.WriteLine("CODIGO: FALTA" + id);
            using (var context = new sistema_notasContext())
            {
                System.Diagnostics.Debug.WriteLine("GRADO: " + "");
                var imagen = (from producto in context.TbGrado
                              where producto.CodGrado == id
                              select producto.Nombre).FirstOrDefault();
                System.Diagnostics.Debug.WriteLine("GRADO: " + "");
                if (imagen == null)
                {
                    return View("No tiene Grado");
                }
                else
                {

                    return View(imagen);
                }

            }
        }

        [Authorize(Roles = "Administrador, Docente")]
        [HttpGet]
        public ActionResult VerEstudiantes(String busqueda)
        {
            ViewData["CurrentFilter"] = busqueda;
            if (!String.IsNullOrEmpty(busqueda))
            {
                var quere = from estudiante in _context.TbUsuario
                            join alumno in _context.TbAlumno on estudiante.CodUsuario equals alumno.CodUsuario
                            join infoP in _context.TbInformacionPersonal on alumno.CodInformacionPersonal equals infoP.CodInformacionPersonal
                            where estudiante.CodRol == 3 && infoP.Nombre.Contains(busqueda)
                            select new
                            {
                                estudiante.Usuario,
                                estudiante.Contraseña,
                                alumno.CodGrado,
                                infoP.Nombre,
                                infoP.Apellido,
                                infoP.CorreoElectronico,
                                infoP.Cui,
                                infoP.Direccion,
                                infoP.Imagen,
                                infoP.Telefono,
                                alumno.Codigo,
                                infoP.CodInformacionPersonal
                            };
                System.Diagnostics.Debug.WriteLine("USuario:" + quere.FirstOrDefault());
                var rol2 = _context.TbUsuario.Where(w => w.CodRol == 3);

                foreach (var item in quere)
                {
                    VerEstudiante estudent = new VerEstudiante();
                    estudent.Apellidos = item.Apellido;
                    estudent.CodGrado = item.CodGrado;
                    estudent.Codigo = item.Codigo;
                    estudent.Correo = item.CorreoElectronico;
                    estudent.Cui = item.Cui;
                    estudent.Direccion = item.Direccion;
                    estudent.Imagen = item.Imagen;
                    estudent.Nombre = item.Nombre;
                    estudent.Telefono = item.Telefono;
                    estudent.Usuario = item.Usuario;
                    estudent.CodInfoPersonal = item.CodInformacionPersonal;
                    listaestud.Add(estudent);
                    System.Diagnostics.Debug.WriteLine("Apellido:" + item.Apellido);
                    System.Diagnostics.Debug.WriteLine("Grado:" + item.CodGrado);


                }
            }
            else
            {
                var quere = from estudiante in _context.TbUsuario
                            join alumno in _context.TbAlumno on estudiante.CodUsuario equals alumno.CodUsuario
                            join infoP in _context.TbInformacionPersonal on alumno.CodInformacionPersonal equals infoP.CodInformacionPersonal
                            where estudiante.CodRol == 3
                            select new
                            {
                                estudiante.Usuario,
                                estudiante.Contraseña,
                                alumno.CodGrado,
                                infoP.Nombre,
                                infoP.Apellido,
                                infoP.CorreoElectronico,
                                infoP.Cui,
                                infoP.Direccion,
                                infoP.Imagen,
                                infoP.Telefono,
                                alumno.Codigo,
                                infoP.CodInformacionPersonal
                            };
                System.Diagnostics.Debug.WriteLine("USuario:" + quere.FirstOrDefault());
                var rol2 = _context.TbUsuario.Where(w => w.CodRol == 3);

                foreach (var item in quere)
                {
                    VerEstudiante estudent = new VerEstudiante();
                    estudent.Apellidos = item.Apellido;
                    estudent.CodGrado = item.CodGrado;
                    estudent.Codigo = item.Codigo;
                    estudent.Correo = item.CorreoElectronico;
                    estudent.Cui = item.Cui;
                    estudent.Direccion = item.Direccion;
                    estudent.Imagen = item.Imagen;
                    estudent.Nombre = item.Nombre;
                    estudent.Telefono = item.Telefono;
                    estudent.Usuario = item.Usuario;
                    estudent.CodInfoPersonal = item.CodInformacionPersonal;
                    listaestud.Add(estudent);

                }
            }
            return View(listaestud);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult VerDocentes(String busqueda)
        {
            ViewData["CurrentFilter"] = busqueda;
            if (!String.IsNullOrEmpty(busqueda))
            {
                var quere = from docente in _context.TbUsuario
                            join profesor in _context.TbDocente on docente.CodUsuario equals profesor.CodUsuario
                            join infoP in _context.TbInformacionPersonal on profesor.CodInformacionPersonal equals infoP.CodInformacionPersonal
                            where docente.CodRol == 2 && infoP.Nombre.Contains(busqueda)
                            select new
                            {
                                docente.Usuario,
                                docente.Contraseña,
                                infoP.Nombre,
                                infoP.Apellido,
                                infoP.CorreoElectronico,
                                infoP.Cui,
                                infoP.Direccion,
                                infoP.Imagen,
                                infoP.Telefono,
                                infoP.CodInformacionPersonal
                            };

                foreach (var item in quere)
                {
                    VerProfesor docentes = new VerProfesor();
                    docentes.Apellidos = item.Apellido;
                    docentes.Correo = item.CorreoElectronico;
                    docentes.Cui = item.Cui;
                    docentes.Direccion = item.Direccion;
                    docentes.Imagen = item.Imagen;
                    docentes.Nombre = item.Nombre;
                    docentes.Telefono = item.Telefono;
                    docentes.Usuario = item.Usuario;
                    docentes.CodInfoPersonal = item.CodInformacionPersonal;
                    listadoc.Add(docentes);

                }
            }
            else
            {
                var quere = from docente in _context.TbUsuario
                            join profesor in _context.TbDocente on docente.CodUsuario equals profesor.CodUsuario
                            join infoP in _context.TbInformacionPersonal on profesor.CodInformacionPersonal equals infoP.CodInformacionPersonal
                            where docente.CodRol == 2
                            select new
                            {
                                docente.Usuario,
                                docente.Contraseña,
                                infoP.Nombre,
                                infoP.Apellido,
                                infoP.CorreoElectronico,
                                infoP.Cui,
                                infoP.Direccion,
                                infoP.Imagen,
                                infoP.Telefono,
                                infoP.CodInformacionPersonal
                            };

                foreach (var item in quere)
                {
                    VerProfesor docentes = new VerProfesor();
                    docentes.Apellidos = item.Apellido;
                    docentes.Correo = item.CorreoElectronico;
                    docentes.Cui = item.Cui;
                    docentes.Direccion = item.Direccion;
                    docentes.Imagen = item.Imagen;
                    docentes.Nombre = item.Nombre;
                    docentes.Telefono = item.Telefono;
                    docentes.Usuario = item.Usuario;
                    docentes.CodInfoPersonal = item.CodInformacionPersonal;
                    listadoc.Add(docentes);

                }
            }
            return View(listadoc);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult VerAdministrador(String busqueda)
        {
            ViewData["CurrentFilter"] = busqueda;
            if (!String.IsNullOrEmpty(busqueda))
            {
                var quere = from admon in _context.TbUsuario
                            join admin in _context.TbAdministracion on admon.CodUsuario equals admin.CodUsuario
                            join infoP in _context.TbInformacionPersonal on admin.CodInformacionPersonal equals infoP.CodInformacionPersonal
                            where admon.CodRol == 1 && infoP.Nombre.Contains(busqueda)
                            select new
                            {
                                admon.Usuario,
                                admon.Contraseña,
                                infoP.Nombre,
                                infoP.Apellido,
                                infoP.CorreoElectronico,
                                infoP.Cui,
                                infoP.Direccion,
                                infoP.Imagen,
                                infoP.Telefono,
                                infoP.CodInformacionPersonal
                            };

                foreach (var item in quere)
                {
                    VerAdministrador admons = new VerAdministrador();
                    admons.Apellidos = item.Apellido;
                    admons.Correo = item.CorreoElectronico;
                    admons.Cui = item.Cui;
                    admons.Direccion = item.Direccion;
                    admons.Imagen = item.Imagen;
                    admons.Nombre = item.Nombre;
                    admons.Telefono = item.Telefono;
                    admons.Usuario = item.Usuario;
                    admons.CodInfoPersonal = item.CodInformacionPersonal;
                    listaadmon.Add(admons);

                }
            }
            else
            {
                var quere = from admon in _context.TbUsuario
                            join admin in _context.TbAdministracion on admon.CodUsuario equals admin.CodUsuario
                            join infoP in _context.TbInformacionPersonal on admin.CodInformacionPersonal equals infoP.CodInformacionPersonal
                            where admon.CodRol == 1
                            select new
                            {
                                admon.Usuario,
                                admon.Contraseña,
                                infoP.Nombre,
                                infoP.Apellido,
                                infoP.CorreoElectronico,
                                infoP.Cui,
                                infoP.Direccion,
                                infoP.Imagen,
                                infoP.Telefono,
                                infoP.CodInformacionPersonal
                            };

                foreach (var item in quere)
                {
                    VerAdministrador admons = new VerAdministrador();
                    admons.Apellidos = item.Apellido;
                    admons.Correo = item.CorreoElectronico;
                    admons.Cui = item.Cui;
                    admons.Direccion = item.Direccion;
                    admons.Imagen = item.Imagen;
                    admons.Nombre = item.Nombre;
                    admons.Telefono = item.Telefono;
                    admons.Usuario = item.Usuario;
                    admons.CodInfoPersonal = item.CodInformacionPersonal;
                    listaadmon.Add(admons);

                }
            }
            return View(listaadmon);
        }

        [Authorize(Roles = "Estudiante")]
        public ActionResult PerfilE()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int CODIGODOC = 0;
            var doc = from Doc in _context.TbAlumno
                      where Doc.CodUsuario == int.Parse(userId)
                      select new
                      {
                          Doc.CodAlumno
                      };

            foreach (var item in doc)
            {
                CODIGODOC = item.CodAlumno;
            }
            var consulta = from Al in _context.TbInformacionPersonal
                           join Al1 in _context.TbAlumno on Al.CodInformacionPersonal equals Al1.CodInformacionPersonal
                           where Al1.CodAlumno == CODIGODOC
                           select new
                           {
                               Nombre = Al.Nombre,
                               Apellido = Al.Apellido,
                               Cui = Al.Cui,
                               Direccion = Al.Direccion,
                               Telefono = Al.Telefono,
                               Imagen = Al.Imagen,
                               Correo = Al.CorreoElectronico,
                               CodInfoPer = Al1.CodInformacionPersonal,
                           };

            foreach (var item in consulta)
            {
                Perfil perfil1 = new Perfil();
                perfil1.Nombre = item.Nombre;
                perfil1.Apellidos = item.Apellido;
                perfil1.Telefono = item.Telefono;
                perfil1.Cui = item.Cui;
                perfil1.Direccion = item.Direccion;
                perfil1.Imagen = item.Imagen;
                perfil1.Correo = item.Correo;
                perfil1.CodInfoPersonal = item.CodInfoPer;
                ListaPerfil.Add(perfil1);
            }
            return View(ListaPerfil);
        }

        [Authorize(Roles = "Estudiante")]
        [HttpPost]
        public async Task<ActionResult> EditarPerfilE(Perfil perf, List<IFormFile> Upload)
        {
            System.Diagnostics.Debug.WriteLine("Nombre:" + perf.Nombre);
            System.Diagnostics.Debug.WriteLine("CodInfoPersonal:" + perf.CodInfoPersonal);

            var customer = _context.TbInformacionPersonal.Where(c => c.CodInformacionPersonal.Equals(perf.CodInfoPersonal)).FirstOrDefault();
            customer.Nombre = perf.Nombre;
            customer.Apellido = perf.Apellidos;
            customer.Telefono = perf.Telefono;
            foreach (var item in Upload)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        perf.Imagen = stream.ToArray();
                    }
                }
            }
            customer.Imagen = perf.Imagen;
            customer.Direccion = perf.Direccion;
            _context.TbInformacionPersonal.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Docente")]
        public ActionResult PerfilD()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int CODIGODOC = 0;
            var doc = from Doc in _context.TbDocente
                      where Doc.CodUsuario == int.Parse(userId)
                      select new
                      {
                          Doc.CodDocente
                      };

            foreach (var item in doc)
            {
                CODIGODOC = item.CodDocente;
            }
            var consulta = from Al in _context.TbInformacionPersonal
                           join Al1 in _context.TbDocente on Al.CodInformacionPersonal equals Al1.CodInformacionPersonal
                           where Al1.CodDocente == CODIGODOC
                           select new
                           {
                               Nombre = Al.Nombre,
                               Apellido = Al.Apellido,
                               Cui = Al.Cui,
                               Direccion = Al.Direccion,
                               Telefono = Al.Telefono,
                               Imagen = Al.Imagen,
                               Correo = Al.CorreoElectronico,
                               CodInfoPer = Al1.CodInformacionPersonal,
                           };

            foreach (var item in consulta)
            {
                Perfil perfil1 = new Perfil();
                perfil1.Nombre = item.Nombre;
                perfil1.Apellidos = item.Apellido;
                perfil1.Telefono = item.Telefono;
                perfil1.Cui = item.Cui;
                perfil1.Direccion = item.Direccion;
                perfil1.Imagen = item.Imagen;
                perfil1.Correo = item.Correo;
                perfil1.CodInfoPersonal = item.CodInfoPer;
                ListaPerfil.Add(perfil1);
            }
            return View(ListaPerfil);
        }

        [Authorize(Roles = "Docente")]
        [HttpPost]
        public async Task<ActionResult> EditarPerfilD(Perfil perf, List<IFormFile> Upload)
        {
            System.Diagnostics.Debug.WriteLine("Nombre:" + perf.Nombre);
            System.Diagnostics.Debug.WriteLine("CodInfoPersonal:" + perf.CodInfoPersonal);

            var customer = _context.TbInformacionPersonal.Where(c => c.CodInformacionPersonal.Equals(perf.CodInfoPersonal)).FirstOrDefault();
            customer.Nombre = perf.Nombre;
            customer.Apellido = perf.Apellidos;
            customer.Telefono = perf.Telefono;
            foreach (var item in Upload)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        perf.Imagen = stream.ToArray();
                    }
                }
            }
            customer.Imagen = perf.Imagen;
            customer.Direccion = perf.Direccion;
            _context.TbInformacionPersonal.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


    }


}
