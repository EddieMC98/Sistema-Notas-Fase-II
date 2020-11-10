using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNotas1.Models;
using SistemaNotas1.Models.Usuarios;
using Rotativa.AspNetCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SistemaNotas1.Controllers
{
	public class TbClasesController : Controller
	{
		private readonly sistema_notasContext _context;
		AgregarClase objItemViewModel = new AgregarClase();
		CursoxDocente objItemViewModel1 = new CursoxDocente();
		public List<VerEstudiante> alumnos = new List<VerEstudiante>();
		public List<CursoxDocente> cursoxDoc = new List<CursoxDocente>();
		public List<CursoxAlum> cursoxAlum = new List<CursoxAlum>();
		public List<VerEstudiante> alumno_clase = new List<VerEstudiante>();
		public List<VerEstudiante> alumno_clase_pdf = new List<VerEstudiante>();
		public List<NotasXAlumno> alumno_notas = new List<NotasXAlumno>();
		public List<VerActividadesxClase> actividadesxClases = new List<VerActividadesxClase>();
		public List<ActividadxClase> numero_actividades = new List<ActividadxClase>();
		public List<VerEstudiante> lista2 = new List<VerEstudiante>();
		public List<int> lista1 = new List<int>();
		public List<NotasGenerales> listanotas = new List<NotasGenerales>();
		public List<TbUnidad> tbJornadas = new List<TbUnidad>();
		public List<int> CodigoClases = new List<int>();
		public List<string> NombreClases = new List<string>();
		public List<int> CodigoUnidades = new List<int>();
		public List<ResultadoGeneral> ResultadoGenerals = new List<ResultadoGeneral>();
		public List<ListadoAlumnos> listados = new List<ListadoAlumnos>();
		public List<int> CodigoClases1 = new List<int>();
		public List<string> NombreClases1 = new List<string>();
		public List<int> CodigoUnidades1 = new List<int>();
		public List<ResultadoGeneral> ResultadoGenerals1 = new List<ResultadoGeneral>();
		private readonly IWebHostEnvironment _env;


		public TbClasesController(sistema_notasContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
			//   LlenarListaAlumnos();
		}

		// GET: TbClases
		public async Task<IActionResult> Index()
		{
			return View(await _context.TbClase.ToListAsync());
		}

		// GET: TbClases/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tbClase = await _context.TbClase
				.FirstOrDefaultAsync(m => m.CodClase == id);
			if (tbClase == null)
			{
				return NotFound();
			}

			return View(tbClase);
		}

		// GET: TbClases/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: TbClases/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CodClase,CodUnidad,CodAño,CodJornada,CodCurso,CodDocente,CodSeccion")] TbClase tbClase)
		{
			if (ModelState.IsValid)
			{
				_context.Add(tbClase);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(tbClase);
		}

		// GET: TbClases/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tbClase = await _context.TbClase.FindAsync(id);
			if (tbClase == null)
			{
				return NotFound();
			}
			return View(tbClase);
		}

		// POST: TbClases/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CodClase,CodUnidad,CodAño,CodJornada,CodCurso,CodDocente,CodSeccion")] TbClase tbClase)
		{
			if (id != tbClase.CodClase)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(tbClase);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TbClaseExists(tbClase.CodClase))
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
			return View(tbClase);
		}

		// GET: TbClases/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var tbClase = await _context.TbClase
				.FirstOrDefaultAsync(m => m.CodClase == id);
			if (tbClase == null)
			{
				return NotFound();
			}

			return View(tbClase);
		}

		// POST: TbClases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var tbClase = await _context.TbClase.FindAsync(id);
			_context.TbClase.Remove(tbClase);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool TbClaseExists(int id)
		{
			return _context.TbClase.Any(e => e.CodClase == id);
		}

		[Authorize(Roles = "Administrador")]
		public ActionResult AgregarClases()
		{
			ViewBag.showSuccessAlert = false;
			objItemViewModel.ListaAño = (from objCat in _context.TbAño
										 select new SelectListItem()
										 {

											 Text = objCat.Numero.ToString(),
											 Value = objCat.CodAño.ToString(),
											 Selected = true
										 });

			objItemViewModel.ListaCurso = (from objCat in _context.TbCurso
										   join objCat1 in _context.TbGrado on objCat.CodGrado equals objCat1.CodGrado
										   join objCat2 in _context.TbNivel on objCat1.CodNivel equals objCat2.CodNivel
										   select new SelectListItem()
										   {

											   Text = String.Concat(objCat.Nombre, " ", objCat1.Nombre, " ", objCat2.Nombre),
											   Value = objCat.CodCurso.ToString(),
											   Selected = true
										   });

			objItemViewModel.ListaUnidad = (from objCat in _context.TbUnidad
											select new SelectListItem()
											{

												Text = objCat.Nombre,
												Value = objCat.CodUnidad.ToString(),
												Selected = true
											});

			objItemViewModel.ListaJornada = (from objCat in _context.TbJornadas
											 select new SelectListItem()
											 {

												 Text = objCat.Nombre,
												 Value = objCat.CodJornada.ToString(),
												 Selected = true
											 });

			objItemViewModel.ListaDocente = (from objCat in _context.TbDocente
											 join objCat1 in _context.TbInformacionPersonal on objCat.CodInformacionPersonal equals objCat1.CodInformacionPersonal

											 select new SelectListItem()
											 {
												 Text = String.Concat(objCat1.Nombre, " ", objCat1.Apellido),
												 Value = objCat.CodDocente.ToString(),
												 Selected = true
											 });

			objItemViewModel.ListaSeccion = (from objCat in _context.TbSeccion
											 select new SelectListItem()
											 {

												 Text = objCat.Nombre,
												 Value = objCat.CodSeccion.ToString(),
												 Selected = true
											 });

			return View(objItemViewModel);
		}

		[Authorize(Roles = "Administrador")]
		[HttpPost]
		public async Task<ActionResult> AgregarClases(AgregarClase useri)
		{
			int codigoal = useri.CodAño ?? default(int);
			int codigoal1 = useri.CodCurso ?? default(int);
			int codigoal2 = useri.CodJornada ?? default(int);
			int codigoal3 = useri.CodDocente ?? default(int);
			int codigoal4 = useri.CodSeccion ?? default(int);
			int codigoal5 = useri.CodUnidad ?? default(int);

			var consulta = from pers in _context.TbClase
						   where pers.CodAño.Equals(useri.CodAño) && pers.CodCurso.Equals(useri.CodCurso)
						   && pers.CodSeccion.Equals(useri.CodSeccion) && pers.CodUnidad.Equals(useri.CodUnidad)
						   && pers.CodJornada.Equals(useri.CodJornada)
						   select new
						   {
							   pers.CodClase,
							   pers.CodAño,
							   pers.CodCurso,
							   pers.CodSeccion,
							   pers.CodJornada,
							   pers.CodUnidad
						   };
			if (consulta.Count() > 0)
			{
				ViewBag.showSuccessAlert = true;
				System.Diagnostics.Debug.WriteLine("ENTRAMOS");
				foreach (var item in consulta)
				{
					System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.CodClase);
				}
				LlenarListaClases();
				return View(objItemViewModel);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("CLASE ASIGNADA");
				TbClase clase = new TbClase();
				clase.CodAño = codigoal;
				clase.CodCurso = codigoal1;
				clase.CodDocente = codigoal3;
				clase.CodJornada = codigoal2;
				clase.CodSeccion = codigoal4;
				clase.CodUnidad = codigoal5;
				_context.TbClase.Add(clase);
				_context.SaveChanges();
			}
			return RedirectToAction("AgregarClases", "TbClases");
		}


		public void LlenarListaClases()
		{


			objItemViewModel.ListaAño = (from objCat in _context.TbAño
										 select new SelectListItem()
										 {

											 Text = objCat.Numero.ToString(),
											 Value = objCat.CodAño.ToString(),
											 Selected = true
										 });

			objItemViewModel.ListaCurso = (from objCat in _context.TbCurso
										   join objCat1 in _context.TbGrado on objCat.CodGrado equals objCat1.CodGrado
										   join objCat2 in _context.TbNivel on objCat1.CodNivel equals objCat2.CodNivel
										   select new SelectListItem()
										   {

											   Text = String.Concat(objCat.Nombre, " ", objCat1.Nombre, " ", objCat2.Nombre),
											   Value = objCat.CodCurso.ToString(),
											   Selected = true
										   });

			objItemViewModel.ListaUnidad = (from objCat in _context.TbUnidad
											select new SelectListItem()
											{

												Text = objCat.Nombre,
												Value = objCat.CodUnidad.ToString(),
												Selected = true
											});

			objItemViewModel.ListaJornada = (from objCat in _context.TbJornadas
											 select new SelectListItem()
											 {

												 Text = objCat.Nombre,
												 Value = objCat.CodJornada.ToString(),
												 Selected = true
											 });

			objItemViewModel.ListaDocente = (from objCat in _context.TbDocente
											 join objCat1 in _context.TbInformacionPersonal on objCat.CodInformacionPersonal equals objCat1.CodInformacionPersonal

											 select new SelectListItem()
											 {
												 Text = String.Concat(objCat1.Nombre, " ", objCat1.Apellido),
												 Value = objCat.CodDocente.ToString(),
												 Selected = true
											 });

			objItemViewModel.ListaSeccion = (from objCat in _context.TbSeccion
											 select new SelectListItem()
											 {

												 Text = objCat.Nombre,
												 Value = objCat.CodSeccion.ToString(),
												 Selected = true
											 });
		}

		[Authorize(Roles = "Docente")]
		public ActionResult VerClases()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
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

			var consulta = from tb_clase in _context.TbClase
						   join tb_jornads in _context.TbJornadas on tb_clase.CodJornada equals tb_jornads.CodJornada
						   join tb_anio in _context.TbAño on tb_clase.CodAño equals tb_anio.CodAño
						   join ojbCat1 in _context.TbUnidad on tb_clase.CodUnidad equals ojbCat1.CodUnidad
						   join tb_seccion in _context.TbSeccion on tb_clase.CodSeccion equals tb_seccion.CodSeccion
						   join curso in _context.TbCurso on tb_clase.CodCurso equals curso.CodCurso
						   join grad in _context.TbGrado on curso.CodGrado equals grad.CodGrado
						   join nive in _context.TbNivel on grad.CodNivel equals nive.CodNivel
						   where tb_clase.CodDocente == CODIGODOC
						   select new
						   {
							   u1 = tb_jornads.Nombre,
							   tb_anio.Numero,
							   u2 = ojbCat1.Nombre,
							   u3 = tb_seccion.Nombre,
							   curso.Nombre,
							   nivel = nive.Nombre,
							   grado = grad.Nombre,
							   tb_clase.CodClase
						   };

			if (consulta.Count() > 0)
			{
				foreach (var item in consulta)
				{
					CursoxDocente cursox = new CursoxDocente();
					cursox.Año = item.Numero.ToString();
					cursox.Unidad = item.u2;
					cursox.Jornada = item.u1;
					cursox.Seccion = item.u3;
					cursox.Curso = item.Nombre;
					cursox.CodClase = item.CodClase;
					cursox.Grado = item.grado;
					cursox.Nivel = item.nivel;
					cursoxDoc.Add(cursox);
					System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.CodClase);
				}
			}


			return View(cursoxDoc);
		}

		//GET:Clases
		[Authorize(Roles = "Docente")]
		[HttpPost]
		//[Authorize(Roles = "Administrador")]
		public ActionResult AsignarClases(string cod_orden, string buscar)
		{
			ViewData["Lista_Alumnos"] = alumnos;
			System.Diagnostics.Debug.WriteLine("CLASE: " + cod_orden);
			SetSession24(cod_orden);
			LlenarListaAlumnos();
			System.Diagnostics.Debug.WriteLine("Alumno: " + buscar);
			return View();

		}

		[Authorize(Roles = "Docente")]
		[HttpPost]
		public ActionResult AsignarAlumnos(VerEstudiante useri)
		{
			int cod_clase = Int32.Parse(GetSession24());
			System.Diagnostics.Debug.WriteLine("Clase A Asignar: " + cod_clase);

			useri.verEstudiantes = limpiarListaAlumnos(useri.verEstudiantes.ToList());
			for (int i = 0; i < useri.verEstudiantes.Count; i++)
			{
				System.Diagnostics.Debug.WriteLine("Estudiante Final: " + (i + 1) + ": " + useri.verEstudiantes.ElementAt(i).CodEstudiante);
				var consulta = from alu1 in _context.TbClaseAlumno
							   where alu1.CodClase == cod_clase && alu1.CodAlumno == useri.verEstudiantes.ElementAt(i).CodEstudiante
							   select new
							   {
								   alu1.CodClase
							   };

				if (consulta.Count() > 0)
				{
					System.Diagnostics.Debug.WriteLine("EL ESTUDIANTE YA ESTA ASIGNADO: " + useri.verEstudiantes.ElementAt(i).CodEstudiante);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("EL ESTUDIANTE NO ESTA ASIGNADO A ESTA CLASE: " + useri.verEstudiantes.ElementAt(i).CodEstudiante);
					TbClaseAlumno tbClaseAlumno = new TbClaseAlumno();
					tbClaseAlumno.CodAlumno = useri.verEstudiantes.ElementAt(i).CodEstudiante;
					tbClaseAlumno.CodClase = cod_clase;
					tbClaseAlumno.Aprobado = 0;
					_context.TbClaseAlumno.Add(tbClaseAlumno);
					_context.SaveChanges();
				}

			}


			return View();
		}

		public void LlenarListaAlumnos()
		{
			bool clase_exists = false;
			int cod_clase = Int32.Parse(GetSession24());
			System.Diagnostics.Debug.WriteLine("Clase del alumno: " + cod_clase);
			int cod_grado_aux = 0;
			var consulta1 = from con in _context.TbClase
							join con1 in _context.TbCurso on con.CodCurso equals con1.CodCurso
							where con.CodClase == cod_clase
							select new
							{
								con1.CodGrado
							};

			foreach (var itm in consulta1)
			{
				cod_grado_aux = itm.CodGrado;
			}

			var consulta = from est in _context.TbAlumno
						   join est1 in _context.TbInformacionPersonal on est.CodInformacionPersonal equals est1.CodInformacionPersonal
						   join est2 in _context.TbGrado on est.CodGrado equals est2.CodGrado
						   join est3 in _context.TbNivel on est2.CodNivel equals est3.CodNivel
						   where est.CodGrado == cod_grado_aux
						   select new
						   {
							   est.CodAlumno,
							   est.Codigo,
							   est1.Nombre,
							   est1.Apellido,
							   est1.CorreoElectronico,
							   est1.Cui,
							   est1.Imagen,
							   est1.Telefono,
							   grado = est2.Nombre,
							   est2.CodGrado,
							   nivel = est3.Nombre
						   };


			foreach (var item in consulta)
			{

				VerEstudiante ola = new VerEstudiante();
				ola.Apellidos = item.Apellido;
				ola.CodGrado = item.CodGrado;
				ola.Codigo = item.Codigo;
				ola.Correo = item.CorreoElectronico;
				ola.Cui = item.Cui;
				ola.Imagen = item.Imagen;
				ola.Nombre = item.Nombre;
				ola.Telefono = item.Telefono;
				ola.Grado = item.grado;
				ola.Nivel = item.nivel;
				ola.CodClase = cod_clase;
				ola.Cod_Estudiante = item.CodAlumno;
				alumnos.Add(ola);
			}

		}

		private List<EstudianteAsignado> limpiarListaAlumnos(List<EstudianteAsignado> inputList)
		{
			Dictionary<int, int> uniqueStore = new Dictionary<int, int>();
			List<EstudianteAsignado> finalList = new List<EstudianteAsignado>();
			foreach (EstudianteAsignado gra in inputList)
			{
				if (!uniqueStore.ContainsKey(gra.CodEstudiante))
				{
					uniqueStore.Add(gra.CodEstudiante, 0);
					finalList.Add(gra);
				}
			}
			return finalList;
		}

		[Authorize(Roles = "Docente")]
		public ActionResult VerAlumnosxClase()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
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

			var consulta = from tb_clase in _context.TbClase
						   join tb_jornads in _context.TbJornadas on tb_clase.CodJornada equals tb_jornads.CodJornada
						   join tb_anio in _context.TbAño on tb_clase.CodAño equals tb_anio.CodAño
						   join ojbCat1 in _context.TbUnidad on tb_clase.CodUnidad equals ojbCat1.CodUnidad
						   join tb_seccion in _context.TbSeccion on tb_clase.CodSeccion equals tb_seccion.CodSeccion
						   join curso in _context.TbCurso on tb_clase.CodCurso equals curso.CodCurso
						   join grad in _context.TbGrado on curso.CodGrado equals grad.CodGrado
						   join nive in _context.TbNivel on grad.CodNivel equals nive.CodNivel
						   where tb_clase.CodDocente == CODIGODOC
						   select new
						   {
							   u1 = tb_jornads.Nombre,
							   tb_anio.Numero,
							   u2 = ojbCat1.Nombre,
							   u3 = tb_seccion.Nombre,
							   curso.Nombre,
							   nivel = nive.Nombre,
							   grado = grad.Nombre,
							   tb_clase.CodClase
						   };

			if (consulta.Count() > 0)
			{
				foreach (var item in consulta)
				{
					CursoxDocente cursox = new CursoxDocente();
					cursox.Año = item.Numero.ToString();
					cursox.Unidad = item.u2;
					cursox.Jornada = item.u1;
					cursox.Seccion = item.u3;
					cursox.Curso = item.Nombre;
					cursox.CodClase = item.CodClase;
					cursox.Grado = item.grado;
					cursox.Nivel = item.nivel;
					cursoxDoc.Add(cursox);
					System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.CodClase);
				}
			}


			return View(cursoxDoc);
		}

		[Authorize(Roles = "Docente")]
		[HttpPost]
		public ActionResult VerAlumnosxClase1(string cod_orden)
		{
			int cod_clase = Int32.Parse(cod_orden);

			var conDoc = from act1 in _context.TbClase
						 join act2 in _context.TbDocente on act1.CodDocente equals act2.CodDocente
						 join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						 select new
						 {
							 NombreD = act3.Nombre,
							 ApellidoD = act3.Apellido
						 };


			var consulta = from act1 in _context.TbClaseAlumno
						   join act2 in _context.TbAlumno on act1.CodAlumno equals act2.CodAlumno
						   join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						   join act4 in _context.TbClase on act1.CodClase equals act4.CodClase
						   join act8 in _context.TbCurso on act4.CodCurso equals act8.CodCurso
						   join act5 in _context.TbJornadas on act4.CodJornada equals act5.CodJornada
						   join act6 in _context.TbSeccion on act4.CodSeccion equals act6.CodSeccion
						   join act7 in _context.TbUnidad on act4.CodUnidad equals act7.CodUnidad
						   join act9 in _context.TbAño on act4.CodAño equals act9.CodAño
						   where act1.CodClase == cod_clase
						   select new
						   {
							   act1.CodAlumno,
							   act1.CodClase,
							   act2.CodInformacionPersonal,
							   act3.Nombre,
							   act3.Apellido,
							   act3.CorreoElectronico,
							   act3.Cui,
							   act3.Imagen,
							   act1.CodClaseAlumno,
							   Jornada = act5.Nombre,
							   Seccion = act6.Nombre,
							   Unidad = act7.Nombre,
							   Curso = act8.Nombre,
							   Año = act9.Numero
						   };



			foreach (var item in consulta)
			{
				VerEstudiante verEstudiante = new VerEstudiante();
				verEstudiante.Nombre = item.Nombre;
				verEstudiante.Apellidos = item.Apellido;
				verEstudiante.CodClase = item.CodClase;
				verEstudiante.Correo = item.CorreoElectronico;
				verEstudiante.Cui = item.Cui;
				verEstudiante.Cod_Estudiante = item.CodAlumno;
				verEstudiante.CodClase = item.CodClase;
				verEstudiante.CodGrado = item.CodClaseAlumno;
				verEstudiante.Clase = item.Curso + " " + item.Seccion + " " + item.Jornada + " " + item.Unidad + " " + item.Año;
				alumno_clase.Add(verEstudiante);
			}

			if (alumno_clase.Count() == 0)
			{
				return RedirectToAction("VerAlumnosxClase", "TbClases");
			}

			else
			{
				foreach (var item in conDoc)
				{
					alumno_clase.ElementAt(0).NombreDoc = item.NombreD + " " + item.ApellidoD;
				}
				return View("VerAlumnosxClasePDF", alumno_clase);
				//return new ViewAsPdf("VerAlumnosxClasePDF", alumno_clase)
				//{
				//    PageSize = Rotativa.AspNetCore.Options.Size.Legal,
				//    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				//};
			}
		}

		[HttpPost]
		[Authorize(Roles = "Docente")]
		public ActionResult ImprimirPDFAlumnos(string cod_clase)
		{

			int cod_clase1 = Int32.Parse(cod_clase);

			var conDoc = from act1 in _context.TbClase
						 join act2 in _context.TbDocente on act1.CodDocente equals act2.CodDocente
						 join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						 select new
						 {
							 NombreD = act3.Nombre,
							 ApellidoD = act3.Apellido
						 };


			var consulta = from act1 in _context.TbClaseAlumno
						   join act2 in _context.TbAlumno on act1.CodAlumno equals act2.CodAlumno
						   join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						   join act4 in _context.TbClase on act1.CodClase equals act4.CodClase
						   join act8 in _context.TbCurso on act4.CodCurso equals act8.CodCurso
						   join act5 in _context.TbJornadas on act4.CodJornada equals act5.CodJornada
						   join act6 in _context.TbSeccion on act4.CodSeccion equals act6.CodSeccion
						   join act7 in _context.TbUnidad on act4.CodUnidad equals act7.CodUnidad
						   join act9 in _context.TbAño on act4.CodAño equals act9.CodAño
						   where act1.CodClase == cod_clase1
						   select new
						   {
							   act1.CodAlumno,
							   act1.CodClase,
							   act2.CodInformacionPersonal,
							   act3.Nombre,
							   act3.Apellido,
							   act3.CorreoElectronico,
							   act3.Cui,
							   act3.Imagen,
							   act1.CodClaseAlumno,
							   Jornada = act5.Nombre,
							   Seccion = act6.Nombre,
							   Unidad = act7.Nombre,
							   Curso = act8.Nombre,
							   Año = act9.Numero
						   };


			foreach (var item in consulta)
			{
				VerEstudiante verEstudiante = new VerEstudiante();
				verEstudiante.Nombre = item.Nombre;
				verEstudiante.Apellidos = item.Apellido;
				verEstudiante.CodClase = item.CodClase;
				verEstudiante.Correo = item.CorreoElectronico;
				verEstudiante.Cui = item.Cui;
				verEstudiante.Cod_Estudiante = item.CodAlumno;
				verEstudiante.CodClase = item.CodClase;
				verEstudiante.CodGrado = item.CodClaseAlumno;
				verEstudiante.Clase = item.Curso + " " + item.Seccion + " " + item.Jornada + " " + item.Unidad + " " + item.Año;
				alumno_clase_pdf.Add(verEstudiante);
			}

			if (alumno_clase_pdf.Count() == 0)
			{
				return RedirectToAction("VerAlumnosxClase", "TbClases");
			}

			else
			{
				foreach (var item in conDoc)
				{
					alumno_clase_pdf.ElementAt(0).NombreDoc = item.NombreD + " " + item.ApellidoD;
				}
			}




			//GENERAR PDF
			Document pdfDoc = new Document();
			pdfDoc.SetPageSize(PageSize.Letter.Rotate());
			pdfDoc.SetMargins(42f, 42f, 42f, 42f);
			//FileStream file = new FileStream("hola_mundo.pdf", FileMode.Create);
			MemoryStream ms = new MemoryStream();
			PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
			pdfDoc.AddAuthor("Eddie Macz");
			pdfDoc.AddTitle("Sistema de Notas");
			pdfDoc.Open();

			//Fuentes

			BaseFont _titulo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
			iTextSharp.text.Font titulo = new Font(_titulo, 18f, Font.BOLD, BaseColor.Black);

			BaseFont _subtitulo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
			iTextSharp.text.Font subtitulo = new Font(_subtitulo, 14f, Font.BOLD, BaseColor.Gray);

			BaseFont _parrafo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
			iTextSharp.text.Font parrafo = new Font(_parrafo, 11f, Font.NORMAL, BaseColor.White);

			BaseFont _parrafo_cuerpo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
			iTextSharp.text.Font parrafocuerpo = new Font(_parrafo_cuerpo, 10f, Font.NORMAL, BaseColor.Black);

			BaseFont _fondo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
			iTextSharp.text.Font fondo = new Font(_fondo, 18f, Font.BOLD, new BaseColor(255, 255, 255));

			//ENCABEZADO

			var tblenc = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100 };
			tblenc.AddCell(new PdfPCell(new Phrase("LISTADO DE ALUMNOS", titulo)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2 });
			tblenc.AddCell(new Phrase(" "));
			pdfDoc.Add(tblenc);

			pdfDoc.Add(Chunk.Newline);
			pdfDoc.Add(Chunk.Newline);
			pdfDoc.Add(Chunk.Newline);
			pdfDoc.Add(Chunk.Newline);

			tblenc = new PdfPTable(new float[] { 7f, 30f, 30f, 33f }) { WidthPercentage = 100 };
			iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "img1", "logo_sf.png"));
			float ancho = logo.Width;
			float alto = logo.Height;
			float proporcion = alto / ancho;

			logo.ScaleAbsolute(80, 80 * proporcion);
			tblenc.AddCell(new PdfPCell(logo) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 3, Colspan = 2 });
			tblenc.AddCell(new PdfPCell(new Phrase("COLEGIO SAGRADA FAMILIA", fondo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2, Padding = 8 });

			tblenc.AddCell(new PdfPCell(new Phrase("LISTADO DE ALUMNOS", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
			tblenc.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

			tblenc.AddCell(new PdfPCell(new Phrase(alumno_clase_pdf.ElementAt(0).Clase, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
			tblenc.AddCell(new PdfPCell(new Phrase("Docente: " + alumno_clase_pdf.ElementAt(0).NombreDoc, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

			tblenc.AddCell(new PdfPCell(new Phrase("No.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
			tblenc.AddCell(new PdfPCell(new Phrase("NOMBRE", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

			tblenc.AddCell(new PdfPCell(new Phrase("CUI", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
			tblenc.AddCell(new PdfPCell(new Phrase("CORREO ", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });


			for (int i = 0; i < alumno_clase_pdf.Count(); i++)
			{
				tblenc.AddCell(new PdfPCell(new Phrase((i + 1).ToString(), parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase(alumno_clase_pdf.ElementAt(i).Nombre + " " + alumno_clase_pdf.ElementAt(i).Apellidos, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase(alumno_clase_pdf.ElementAt(i).Cui, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase(alumno_clase_pdf.ElementAt(i).Correo, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
			}

			tblenc.AddCell(new PdfPCell(new Phrase("TOTAL DE ALUMNOS", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 3 });
			tblenc.AddCell(new PdfPCell(new Phrase(alumno_clase_pdf.Count + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
			pdfDoc.Add(tblenc);

			pdfWriter.Close();
			pdfDoc.Close();
			//ms.Seek(0, SeekOrigin.Begin);
			//var pdf = new FileStream("hola_mundo.pdf", FileMode.Open, FileAccess.Read);
			return File(ms.ToArray(), "application/pdf");
		}


		[Authorize(Roles = "Docente")]
		public ActionResult VerListados()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
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

			var consulta = from tb_clase in _context.TbClase
						   join tb_jornads in _context.TbJornadas on tb_clase.CodJornada equals tb_jornads.CodJornada
						   join tb_anio in _context.TbAño on tb_clase.CodAño equals tb_anio.CodAño
						   join ojbCat1 in _context.TbUnidad on tb_clase.CodUnidad equals ojbCat1.CodUnidad
						   join tb_seccion in _context.TbSeccion on tb_clase.CodSeccion equals tb_seccion.CodSeccion
						   join curso in _context.TbCurso on tb_clase.CodCurso equals curso.CodCurso
						   join grad in _context.TbGrado on curso.CodGrado equals grad.CodGrado
						   join nive in _context.TbNivel on grad.CodNivel equals nive.CodNivel
						   where tb_clase.CodDocente == CODIGODOC
						   select new
						   {
							   u1 = tb_jornads.Nombre,
							   tb_anio.Numero,
							   u2 = ojbCat1.Nombre,
							   u3 = tb_seccion.Nombre,
							   curso.Nombre,
							   nivel = nive.Nombre,
							   grado = grad.Nombre,
							   tb_clase.CodClase
						   };

			if (consulta.Count() > 0)
			{
				foreach (var item in consulta)
				{
					CursoxDocente cursox = new CursoxDocente();
					cursox.Año = item.Numero.ToString();
					cursox.Unidad = item.u2;
					cursox.Jornada = item.u1;
					cursox.Seccion = item.u3;
					cursox.Curso = item.Nombre;
					cursox.CodClase = item.CodClase;
					cursox.Grado = item.grado;
					cursox.Nivel = item.nivel;
					cursoxDoc.Add(cursox);
					System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.CodClase);
				}
			}


			return View(cursoxDoc);
		}

		[Authorize(Roles = "Docente")]
		[HttpPost]
		public ActionResult VerListadoAlumnos(string cod_orden)
		{

			System.Diagnostics.Debug.WriteLine("Codigo de Clase Asignada: " + cod_orden);
			int cod_clase = Int32.Parse(cod_orden);

			var conDoc = from act1 in _context.TbClase
						 join act2 in _context.TbDocente on act1.CodDocente equals act2.CodDocente
						 join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						 select new
						 {
							 NombreD = act3.Nombre,
							 ApellidoD = act3.Apellido
						 };


			var consulta = from act1 in _context.TbClaseAlumno
						   join act2 in _context.TbAlumno on act1.CodAlumno equals act2.CodAlumno
						   join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						   join act4 in _context.TbClase on act1.CodClase equals act4.CodClase
						   join act8 in _context.TbCurso on act4.CodCurso equals act8.CodCurso
						   join act5 in _context.TbJornadas on act4.CodJornada equals act5.CodJornada
						   join act6 in _context.TbSeccion on act4.CodSeccion equals act6.CodSeccion
						   join act7 in _context.TbUnidad on act4.CodUnidad equals act7.CodUnidad
						   join act9 in _context.TbAño on act4.CodAño equals act9.CodAño
						   where act1.CodClase == cod_clase
						   select new
						   {
							   act1.CodAlumno,
							   act1.CodClase,
							   act2.CodInformacionPersonal,
							   act3.Nombre,
							   act3.Apellido,
							   act3.CorreoElectronico,
							   act3.Cui,
							   act3.Imagen,
							   act1.CodClaseAlumno,
							   Jornada = act5.Nombre,
							   Seccion = act6.Nombre,
							   Unidad = act7.Nombre,
							   Curso = act8.Nombre,
							   Año = act9.Numero
						   };



			foreach (var item in consulta)
			{
				VerEstudiante verEstudiante = new VerEstudiante();
				verEstudiante.Nombre = item.Nombre;
				verEstudiante.Apellidos = item.Apellido;
				verEstudiante.CodClase = item.CodClase;
				verEstudiante.Correo = item.CorreoElectronico;
				verEstudiante.Cui = item.Cui;
				verEstudiante.Cod_Estudiante = item.CodAlumno;
				verEstudiante.CodClase = item.CodClase;
				verEstudiante.CodGrado = item.CodClaseAlumno;
				verEstudiante.Clase = item.Curso + " " + item.Seccion + " " + item.Jornada + " " + item.Unidad + " " + item.Año;
				alumno_clase.Add(verEstudiante);
			}

			if (alumno_clase.Count() == 0)
			{
				return RedirectToAction("VerListados", "TbClases");
			}
			else
			{
				foreach (var item in conDoc)
				{
					alumno_clase.ElementAt(0).NombreDoc = item.NombreD + " " + item.ApellidoD;
				}

				//  return View("VerAlumnosxClasePDF", alumno_clase);
				return new ViewAsPdf("VerListadoAlumnos", alumno_clase)
				{
					PageSize = Rotativa.AspNetCore.Options.Size.Legal,
					PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				};
			}
		}

		[Authorize(Roles = "Docente")]
		public async Task<IActionResult> DeleteAlumno(int? id, int? id2)
		{
			System.Diagnostics.Debug.WriteLine("CodAlumno: " + id + ", CodClasE: " + id2);

			if (id == null)
			{
				return NotFound();
			}

			var tbClase = await _context.TbClaseAlumno
				.FirstOrDefaultAsync(m => m.CodClase == id2 && m.CodAlumno == id);
			if (tbClase == null)
			{
				return NotFound();
			}

			return View(tbClase);
		}

		[Authorize(Roles = "Docente")]
		// POST: TbClases/Delete/5
		[HttpPost, ActionName("DeleteAlumno")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmedAlumno(int CodClaseAlumno)
		{
			var tbClase = await _context.TbClaseAlumno.FindAsync(CodClaseAlumno);
			_context.TbClaseAlumno.Remove(tbClase);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(VerAlumnosxClase));
		}


		public void SetSession24(string obj)
		{
			HttpContext.Session.SetString("direccion2", obj);
		}

		public string GetSession24()
		{
			return HttpContext.Session.GetString("direccion2");
		}

		[Authorize(Roles = "Estudiante")]
		public ActionResult VerUnidades()
		{
			var consulta = from jornada in _context.TbUnidad
						   select new
						   {
							   jornada.Nombre,
							   jornada.CodUnidad
						   };
			foreach (var item in consulta)
			{
				TbUnidad jornadaaux = new TbUnidad();
				jornadaaux.Nombre = item.Nombre;
				jornadaaux.CodUnidad = item.CodUnidad;
				tbJornadas.Add(jornadaaux);
			}
			return View(tbJornadas);
		}

		[Authorize(Roles = "Estudiante")]
		[HttpPost]
		public ActionResult VerClase_Alumno(string cod_orden)
		{
			int CodUnidad2 = Int32.Parse(cod_orden);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
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

			var consulta = from tb_clase in _context.TbClaseAlumno
						   join tb_clase1 in _context.TbClase on tb_clase.CodClase equals tb_clase1.CodClase
						   join tb_jornads in _context.TbJornadas on tb_clase1.CodJornada equals tb_jornads.CodJornada
						   join tb_anio in _context.TbAño on tb_clase1.CodAño equals tb_anio.CodAño
						   join ojbCat1 in _context.TbUnidad on tb_clase1.CodUnidad equals ojbCat1.CodUnidad
						   join tb_seccion in _context.TbSeccion on tb_clase1.CodSeccion equals tb_seccion.CodSeccion
						   join curso in _context.TbCurso on tb_clase1.CodCurso equals curso.CodCurso
						   join grad in _context.TbGrado on curso.CodGrado equals grad.CodGrado
						   join nive in _context.TbNivel on grad.CodNivel equals nive.CodNivel
						   where tb_clase.CodAlumno == CODIGODOC && tb_clase1.CodUnidad == CodUnidad2
						   select new
						   {
							   u1 = tb_jornads.Nombre,
							   tb_anio.Numero,
							   u2 = ojbCat1.Nombre,
							   u3 = tb_seccion.Nombre,
							   curso.Nombre,
							   nivel = nive.Nombre,
							   grado = grad.Nombre,
							   tb_clase.CodClase
						   };

			if (consulta.Count() > 0)
			{
				foreach (var item in consulta)
				{
					CursoxAlum cursox = new CursoxAlum();
					cursox.Año = item.Numero.ToString();
					cursox.Unidad = item.u2;
					cursox.Jornada = item.u1;
					cursox.Seccion = item.u3;
					cursox.Curso = item.Nombre;
					cursox.CodClase = item.CodClase;
					cursox.Grado = item.grado;
					cursox.Nivel = item.nivel;
					cursoxAlum.Add(cursox);
					System.Diagnostics.Debug.WriteLine("Registro Similar: " + item.CodClase);
				}
			}


			return View(cursoxAlum);
		}

		[Authorize(Roles = "Estudiante")]
		[HttpPost]
		public ActionResult VerClase_Alumno_Actividades(string cod_orden)
		{

			decimal Total = 0;
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
			int CODIGODOC = 0;
			string NombreA = "";
			string ApellidoA = "";
			var doc = from Doc in _context.TbAlumno
					  join doc1 in _context.TbInformacionPersonal on Doc.CodInformacionPersonal equals doc1.CodInformacionPersonal
					  where Doc.CodUsuario == int.Parse(userId)
					  select new
					  {
						  Doc.CodAlumno,
						  Nombre = doc1.Nombre,
						  Apellido = doc1.Apellido
					  };

			foreach (var item in doc)
			{
				CODIGODOC = item.CodAlumno;
				NombreA = item.Nombre;
				ApellidoA = item.Apellido;
			}
			int cod_clase = Int32.Parse(cod_orden);
			var consulta = from cons1 in _context.TbActividad
						   join cons2 in _context.TbNota on cons1.CodActividad equals cons2.CodActividad
						   join cons3 in _context.TbClase on cons1.CodClase equals cons3.CodClase
						   join cons4 in _context.TbCurso on cons3.CodCurso equals cons4.CodCurso
						   join cons5 in _context.TbGrado on cons4.CodGrado equals cons5.CodGrado
						   join cons6 in _context.TbNivel on cons5.CodNivel equals cons6.CodNivel
						   join cons7 in _context.TbJornadas on cons3.CodJornada equals cons7.CodJornada
						   join cons8 in _context.TbAño on cons3.CodAño equals cons8.CodAño
						   join cons9 in _context.TbSeccion on cons3.CodSeccion equals cons9.CodSeccion
						   join cons10 in _context.TbUnidad on cons3.CodUnidad equals cons10.CodUnidad
						   join cons11 in _context.TbDocente on cons3.CodDocente equals cons11.CodDocente
						   join cons12 in _context.TbInformacionPersonal on cons11.CodInformacionPersonal equals cons12.CodInformacionPersonal

						   where cons1.CodClase == cod_clase && cons2.CodAlumno == CODIGODOC
						   select new
						   {
							   cons1.CodClase,
							   cons1.Descripcion,
							   cons1.Nombre,
							   CodActividad = cons1.CodActividad,
							   Nota = cons1.Punteo,
							   cons2.Punteo,
							   NombreDoc = cons12.Nombre,
							   Apellido = cons12.Apellido,
							   Grado = cons5.Nombre,
							   Seccion = cons9.Nombre,
							   Año = cons8.Numero,
							   Jornada = cons7.Nombre,
							   Nivel = cons6.Nombre,
							   Curso = cons4.Nombre,
							   Unidad = cons10.Nombre
						   };
			if (consulta.Count() > 0)
			{
				VerActividadesxClase lista123 = new VerActividadesxClase();
				lista123.actividad_punteo = new List<ActividadxClase>();
				foreach (var item in consulta)
				{
					ActividadxClase lisa = new ActividadxClase();
					lisa.Punteo = item.Punteo ?? default(decimal);
					System.Diagnostics.Debug.WriteLine("PUNTEO GUARDADO 1: " + lisa.Punteo);
					VerActividadesxClase actxclase = new VerActividadesxClase();
					System.Diagnostics.Debug.WriteLine("PUNTEO GUARDADO 2: " + lisa.Punteo);
					actxclase.CodClase = item.CodClase;
					actxclase.Descripcion = item.Descripcion;
					actxclase.Nombre = item.Nombre;
					actxclase.Punteo = item.Nota;
					actxclase.CodActividad = item.CodActividad;
					lista123.actividad_punteo.Add(lisa);
					System.Diagnostics.Debug.WriteLine("PUNTEO GUARDADO 3: " + lisa.Punteo);
					Total += lisa.Punteo;
					actxclase.actividad_punteo = lista123.actividad_punteo;
					actxclase.Total = Total;
					actxclase.NombreDoc = item.NombreDoc + " " + item.Apellido;
					actxclase.Seccion = item.Seccion;
					actxclase.Unidad = item.Unidad;
					actxclase.Grado = item.Grado;
					actxclase.Jornada = item.Jornada;
					actxclase.Clase = item.Curso;
					actxclase.Año = item.Año + "";
					actxclase.Alumno = NombreA + " " + ApellidoA;
					actividadesxClases.Add(actxclase);

				}
			}


			if (actividadesxClases.Count() == 0)
			{
				return RedirectToAction("VerUnidades", "TbClases");
			}
			else
			{

				//DateTime nombre = DateTime.Now;
				//return View(actividadesxClases);
				//return View("GenerarPDFA",actividadesxClases);

				//GENERAR PDF
				Document pdfDoc = new Document();
				pdfDoc.SetPageSize(PageSize.Letter.Rotate());
				pdfDoc.SetMargins(42f, 42f, 42f, 42f);
				//FileStream file = new FileStream("hola_mundo.pdf", FileMode.Create);
				MemoryStream ms = new MemoryStream();
				PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
				pdfDoc.AddAuthor("Eddie Macz");
				pdfDoc.AddTitle("Sistema de Notas");
				pdfDoc.Open();

				//Fuentes

				BaseFont _titulo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font titulo = new Font(_titulo, 18f, Font.BOLD, BaseColor.Black);

				BaseFont _subtitulo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font subtitulo = new Font(_subtitulo, 14f, Font.BOLD, BaseColor.Gray);

				BaseFont _parrafo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafo = new Font(_parrafo, 11f, Font.NORMAL, BaseColor.White);

				BaseFont _parrafo_cuerpo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafocuerpo = new Font(_parrafo_cuerpo, 10f, Font.NORMAL, BaseColor.Black);

				BaseFont _fondo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font fondo = new Font(_fondo, 18f, Font.BOLD, new BaseColor(255, 255, 255));

				//ENCABEZADO

				var tblenc = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100 };
				tblenc.AddCell(new PdfPCell(new Phrase("NOTA DE ACTIVIDADES", titulo)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2 });
				tblenc.AddCell(new Phrase(" "));
				pdfDoc.Add(tblenc);

				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);

				tblenc = new PdfPTable(new float[] { 7f, 10f, 24f, 20f, 14f, 20f }) { WidthPercentage = 100 };
				iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "img1", "logo_sf.png"));
				float ancho = logo.Width;
				float alto = logo.Height;
				float proporcion = alto / ancho;

				logo.ScaleAbsolute(80, 80 * proporcion);
				tblenc.AddCell(new PdfPCell(logo) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 3, Colspan = 2 });
				tblenc.AddCell(new PdfPCell(new Phrase("COLEGIO SAGRADA FAMILIA", fondo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 4, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Alumno: " + actividadesxClases.ElementAt(0).Alumno, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 3 });
				tblenc.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 1 });

				tblenc.AddCell(new PdfPCell(new Phrase("Clase: " + actividadesxClases.ElementAt(0).Clase, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 3 });
				tblenc.AddCell(new PdfPCell(new Phrase("Docente: " + actividadesxClases.ElementAt(0).NombreDoc, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 1 });

				tblenc.AddCell(new PdfPCell(new Phrase("No.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Codigo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Nombre", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Descripción ", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Punteo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Nota", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				for (int i = 0; i < actividadesxClases.Count(); i++)
				{
					tblenc.AddCell(new PdfPCell(new Phrase((i + 1).ToString(), parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase("ACT -" + actividadesxClases.ElementAt(i).CodActividad, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

					tblenc.AddCell(new PdfPCell(new Phrase(actividadesxClases.ElementAt(i).Nombre, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(actividadesxClases.ElementAt(i).Descripcion, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(actividadesxClases.ElementAt(i).Punteo + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(actividadesxClases.ElementAt(i).actividad_punteo.ElementAt(i).Punteo + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				}

				tblenc.AddCell(new PdfPCell(new Phrase("Total ", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 5 });
				tblenc.AddCell(new PdfPCell(new Phrase(actividadesxClases.ElementAt(actividadesxClases.Count - 1).Total + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				pdfDoc.Add(tblenc);

				pdfWriter.Close();
				pdfDoc.Close();
				//ms.Seek(0, SeekOrigin.Begin);
				//var pdf = new FileStream("hola_mundo.pdf", FileMode.Open, FileAccess.Read);
				return File(ms.ToArray(), "application/pdf");

				//return new ViewAsPdf("GenerarPDFA", actividadesxClases)
				//{
				//	//FileName = "" + nombre+".pdf",
				//	PageSize = Rotativa.AspNetCore.Options.Size.Legal,
				//	PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				//};
			}
		}

		[Authorize(Roles = "Docente")]
		public ActionResult VerResultadosNotas()
		{
			var consulta = from jornada in _context.TbUnidad
						   select new
						   {
							   jornada.Nombre,
							   jornada.CodUnidad
						   };
			foreach (var item in consulta)
			{
				TbUnidad jornadaaux = new TbUnidad();
				jornadaaux.Nombre = item.Nombre;
				jornadaaux.CodUnidad = item.CodUnidad;
				tbJornadas.Add(jornadaaux);
			}
			return View(tbJornadas);
		}

		[Authorize(Roles = "Docente")]
		[HttpPost]
		public ActionResult VerNotas(string cod_orden)
		{
			int codUnidad1 = Int32.Parse(cod_orden);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
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

			var consulta = from tb_clase in _context.TbClase
						   join tb_jornads in _context.TbJornadas on tb_clase.CodJornada equals tb_jornads.CodJornada
						   join tb_anio in _context.TbAño on tb_clase.CodAño equals tb_anio.CodAño
						   join ojbCat1 in _context.TbUnidad on tb_clase.CodUnidad equals ojbCat1.CodUnidad
						   join tb_seccion in _context.TbSeccion on tb_clase.CodSeccion equals tb_seccion.CodSeccion
						   join curso in _context.TbCurso on tb_clase.CodCurso equals curso.CodCurso
						   join grad in _context.TbGrado on curso.CodGrado equals grad.CodGrado
						   join nive in _context.TbNivel on grad.CodNivel equals nive.CodNivel
						   where tb_clase.CodDocente == CODIGODOC && tb_clase.CodUnidad == codUnidad1
						   select new
						   {
							   u1 = tb_jornads.Nombre,
							   tb_anio.Numero,
							   u2 = ojbCat1.Nombre,
							   u3 = tb_seccion.Nombre,
							   curso.Nombre,
							   nivel = nive.Nombre,
							   grado = grad.Nombre,
							   tb_clase.CodClase
						   };

			if (consulta.Count() > 0)
			{
				foreach (var item in consulta)
				{
					CursoxDocente cursox = new CursoxDocente();
					cursox.Año = item.Numero.ToString();
					cursox.Unidad = item.u2;
					cursox.Jornada = item.u1;
					cursox.Seccion = item.u3;
					cursox.Curso = item.Nombre;
					cursox.CodClase = item.CodClase;
					cursox.Grado = item.grado;
					cursox.Nivel = item.nivel;
					cursoxDoc.Add(cursox);
				}
			}

			if (cursoxDoc.Count() == 0)
			{
				return RedirectToAction("VerResultadosNotas", "TbClases");
			}
			else
			{
				return View(cursoxDoc);
			}
		}

		[Authorize(Roles = "Docente")]
		[HttpPost]
		public ActionResult Notas(string cod_orden)
		{
			int cod_clase = Int32.Parse(cod_orden);
			int cont_act;

			var nombre = from cons1 in _context.TbClase
						 join cons2 in _context.TbCurso on cons1.CodCurso equals cons2.CodCurso
						 join cons6 in _context.TbGrado on cons2.CodGrado equals cons6.CodGrado
						 join cons7 in _context.TbNivel on cons6.CodNivel equals cons7.CodNivel
						 join cons3 in _context.TbSeccion on cons1.CodSeccion equals cons3.CodSeccion
						 join cons4 in _context.TbAño on cons1.CodAño equals cons4.CodAño
						 join cons5 in _context.TbJornadas on cons1.CodJornada equals cons5.CodJornada
						 join cons8 in _context.TbUnidad on cons1.CodUnidad equals cons8.CodUnidad
						 where cons1.CodClase == cod_clase
						 select new
						 {
							 Curso = cons2.Nombre,
							 Grado = cons6.Nombre,
							 Nivel = cons7.Nombre,
							 Seccion = cons3.Nombre,
							 Año = cons4.Numero,
							 Jornada = cons5.Nombre,
							 Unidad = cons8.Nombre,
							 CodDoce = cons1.CodDocente
						 };

			string NombreClase = nombre.FirstOrDefault().Curso + " " + nombre.FirstOrDefault().Grado + " " + nombre.FirstOrDefault().Nivel + " " +
				nombre.FirstOrDefault().Seccion + " " + nombre.FirstOrDefault().Jornada;

			int CodDoc1 = nombre.FirstOrDefault().CodDoce;

			string NombreUnidad = nombre.FirstOrDefault().Unidad;

			var doc1 = from fms in _context.TbDocente
					   join fms2 in _context.TbInformacionPersonal on fms.CodInformacionPersonal equals fms2.CodInformacionPersonal
					   where fms.CodDocente == CodDoc1
					   select new
					   {
						   Nombre = fms2.Nombre,
						   Apellido = fms2.Apellido
					   };

			string NombreProfesor = doc1.FirstOrDefault().Nombre + " " + doc1.FirstOrDefault().Apellido;

			var consulta = from act1 in _context.TbNota
						   join act2 in _context.TbAlumno on act1.CodAlumno equals act2.CodAlumno
						   join act3 in _context.TbInformacionPersonal on act2.CodInformacionPersonal equals act3.CodInformacionPersonal
						   join act5 in _context.TbActividad on act1.CodActividad equals act5.CodActividad
						   where act5.CodClase == cod_clase
						   select new
						   {
							   act1.Punteo,
							   act2.CodAlumno,
							   act3.Nombre,
							   act3.Apellido,
							   act2.Codigo
						   };


			//Consulta para ver cuántas actividades tiene la clase
			var consulta3 = from act2 in _context.TbActividad
							where act2.CodClase == cod_clase
							select new
							{
								act2.CodActividad
							};

			cont_act = consulta3.Count();
			foreach (var item in consulta3)
			{
				lista1.Add(item.CodActividad);
			}

			System.Diagnostics.Debug.WriteLine("Total de Actividades de la Clase: " + consulta3.Count() + ", Cantidad de Actividades:" + lista1.Count);
			System.Diagnostics.Debug.WriteLine("Total de Punteos: " + consulta.Count());
			var consulta2 = consulta.GroupBy(x => new { x.CodAlumno, x.Nombre, x.Apellido, x.Codigo }).Select(x => new
			{
				Nombre = x.Key.Nombre,
				Apellido = x.Key.Apellido,
				Codigo = x.Key.Codigo,
				CodAlumno = x.Key.CodAlumno,
				Total = x.Sum(y => y.Punteo)
			}).ToList();

			for (int i = 0; i < consulta2.Count; i++)
			{
				System.Diagnostics.Debug.WriteLine("Nombre: " + consulta2[i].Nombre + " " + consulta2[i].Apellido);
				foreach (var item in consulta3)
				{
					ActividadxClase hola = new ActividadxClase();
					hola.CodActividad = item.CodActividad;
					hola.CodAlumno = consulta2[i].CodAlumno;
					hola.CodClase = cod_clase;
					numero_actividades.Add(hola);
				}

				System.Diagnostics.Debug.WriteLine("Codigo: " + consulta2[i].Codigo);
				System.Diagnostics.Debug.WriteLine("Punteo: " + consulta2[i].Total);
				NotasXAlumno veralumno = new NotasXAlumno();

				veralumno.Nombre = consulta2[i].Nombre;
				veralumno.Apellidos = consulta2[i].Apellido;
				veralumno.Codigo = consulta2[i].Codigo;
				veralumno.Total = consulta2[i].Total ?? default(decimal);
				if (veralumno.Total > 60)
				{
					veralumno.Estado = "Aprobado";
				}
				else
				{
					veralumno.Estado = "Reprobado";
				}
				veralumno.contador = cont_act;
				alumno_notas.Add(veralumno);
			}
			int contador_aux = 0;
			for (int j = 0; j < numero_actividades.Count; j++)
			{
				VerEstudiante lista3 = new VerEstudiante();
				lista3.Cod_Estudiante = numero_actividades.ElementAt(j).CodAlumno;
				lista3.CodClase = numero_actividades.ElementAt(j).CodClase;
				System.Diagnostics.Debug.WriteLine("Estudiante numero: " + numero_actividades.ElementAt(j).CodAlumno + ", Actividad: " + numero_actividades.ElementAt(j).CodActividad +
					", Clase: " + cod_clase + ", Punteo: ");
				lista3.actividad_punteo = new List<ActividadxClase>();

				for (int k = 0; k < lista1.Count; k++)
				{
					ActividadxClase actividad = new ActividadxClase();
					actividad.Punteo = Resultado_Actividad(numero_actividades.ElementAt(j).CodAlumno, numero_actividades.ElementAt(k).CodActividad, cod_clase);

					lista3.actividad_punteo.Add(actividad);

					//System.Diagnostics.Debug.WriteLine("Punteo: " + Resultado_Actividad(numero_actividades.ElementAt(j).CodAlumno, numero_actividades.ElementAt(k).CodActividad, cod_clase));

				}
				alumno_notas.ElementAt(contador_aux).actividad_punteo = lista3.actividad_punteo;

				contador_aux++;
				j = (cont_act * contador_aux) - 1;
				lista2.Add(lista3);
			}

			//System.Diagnostics.Debug.WriteLine("LISTA DE NOTAS GENERALES OLA K ASE");
			//for (int l = 0; l < lista2.Count; l++)
			//{
			//	System.Diagnostics.Debug.WriteLine("CodAlumno: " + lista2.ElementAt(l).Cod_Estudiante);
			//	for (int m = 0; m < lista2.ElementAt(l).actividad_punteo.Count; m++)
			//	{
			//		System.Diagnostics.Debug.WriteLine("Actividad" + (m + 1) + ": " + lista2.ElementAt(l).actividad_punteo.ElementAt(m).Punteo);
			//	}
			//}
			ViewData["Lista_Alumnos2"] = cont_act;

			//System.Diagnostics.Debug.WriteLine("Contador de indices: " + alumno_notas.ElementAt(1).actividad_punteo.Count);
			ViewData["Lista_Alumnos3"] = alumno_notas.Count;

			if (alumno_notas.Count() == 0)
			{
				return RedirectToAction("VerResultadosNotas", "TbClases");
			}
			else
			{
				//GENERAR PDF
				Document pdfDoc = new Document();
				pdfDoc.SetPageSize(PageSize.Letter.Rotate());
				pdfDoc.SetMargins(45f, 45f, 45f, 45f);
				//FileStream file = new FileStream("hola_mundo.pdf", FileMode.Create);
				MemoryStream ms = new MemoryStream();
				PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
				pdfDoc.AddAuthor("Eddie Macz");
				pdfDoc.AddTitle("Sistema de Notas");
				pdfDoc.Open();

				//Fuentes

				BaseFont _titulo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font titulo = new Font(_titulo, 18f, Font.BOLD, BaseColor.Black);

				BaseFont _subtitulo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font subtitulo = new Font(_subtitulo, 14f, Font.BOLD, BaseColor.Gray);

				BaseFont _parrafo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafo = new Font(_parrafo, 9f, Font.NORMAL, BaseColor.White);

				BaseFont _parrafo_cuerpo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafocuerpo = new Font(_parrafo_cuerpo, 8f, Font.NORMAL, BaseColor.Black);

				BaseFont _fondo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font fondo = new Font(_fondo, 18f, Font.BOLD, new BaseColor(255, 255, 255));

				//ENCABEZADO
				int num_act = cont_act;

				float[] columnas = new float[num_act + 5];
				columnas[0] = 7f;
				columnas[1] = 8f;
				columnas[2] = 20;
				columnas[columnas.Length - 2] = 10f;
				columnas[columnas.Length - 1] = 10f;
				for (int j = 3; j < columnas.Length - 2; j++)
				{
					columnas[j] = 10f;
				}

				var tblenc = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100 };
				tblenc.AddCell(new PdfPCell(new Phrase("LISTADO DE NOTAS", titulo)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2 });
				tblenc.AddCell(new Phrase(" "));
				pdfDoc.Add(tblenc);

				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);



				tblenc = new PdfPTable(columnas) { WidthPercentage = 100 };
				iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "img1", "logo_sf.png"));
				float ancho = logo.Width;
				float alto = logo.Height;
				float proporcion = alto / ancho;

				logo.ScaleAbsolute(60, 60 * proporcion);
				tblenc.AddCell(new PdfPCell(logo) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 3, Colspan = 2 });
				tblenc.AddCell(new PdfPCell(new Phrase("COLEGIO SAGRADA FAMILIA", fondo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = columnas.Length - 2, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Clase: " + NombreClase, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = num_act + 1 });
				tblenc.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 2 });

				tblenc.AddCell(new PdfPCell(new Phrase("Docente: " + NombreProfesor, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = num_act + 1 });
				tblenc.AddCell(new PdfPCell(new Phrase("Unidad: " + NombreUnidad, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 2 });

				tblenc.AddCell(new PdfPCell(new Phrase("No.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Codigo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Nombre", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				for (int k = 1; k <= num_act; k++)
				{
					tblenc.AddCell(new PdfPCell(new Phrase("Actividad " + k, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				}

				tblenc.AddCell(new PdfPCell(new Phrase("Punteo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Estado", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				System.Diagnostics.Debug.WriteLine("LISTADO DE NOTAS GENERALES ");
				for (int l = 0; l < alumno_notas.Count; l++)
				{
					System.Diagnostics.Debug.WriteLine("CodAlumno: " + alumno_notas.ElementAt(l).Cod_Estudiante);
					tblenc.AddCell(new PdfPCell(new Phrase((l + 1).ToString(), parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(alumno_notas.ElementAt(l).Codigo, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(alumno_notas.ElementAt(l).Nombre + " " + alumno_notas.ElementAt(l).Apellidos, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					for (int m = 0; m < alumno_notas.ElementAt(l).actividad_punteo.Count; m++)
					{
						//	System.Diagnostics.Debug.WriteLine("Actividad" + (m + 1) + ": " + alumno_notas.ElementAt(l).actividad_punteo.ElementAt(m).Punteo);
						tblenc.AddCell(new PdfPCell(new Phrase(alumno_notas.ElementAt(l).actividad_punteo.ElementAt(m).Punteo + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					}
					tblenc.AddCell(new PdfPCell(new Phrase(alumno_notas.ElementAt(l).Total + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(alumno_notas.ElementAt(l).Estado, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				}




				pdfDoc.Add(tblenc);

				pdfWriter.Close();
				pdfDoc.Close();
				//ms.Seek(0, SeekOrigin.Begin);
				//var pdf = new FileStream("hola_mundo.pdf", FileMode.Open, FileAccess.Read);
				return File(ms.ToArray(), "application/pdf");


				//return View(alumno_notas);
				//return new ViewAsPdf("GenerarPDF", alumno_notas)
				//{
				//	PageSize = Rotativa.AspNetCore.Options.Size.Legal,
				//	PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				//};
			}
		}

		public decimal Resultado_Actividad(int codalumno, int cod_actividad, int codigo)
		{
			decimal punteo = 0;
			var consulta = from act in _context.TbAlumno
						   join act1 in _context.TbNota on act.CodAlumno equals act1.CodAlumno
						   join act2 in _context.TbActividad on act1.CodActividad equals act2.CodActividad
						   where act2.CodClase == codigo && act.CodAlumno == codalumno && act1.CodActividad == cod_actividad
						   select new
						   {
							   act1.Punteo
						   };

			int contador = consulta.Count();

			if (contador > 0)
			{
				foreach (var item in consulta)
				{
					punteo = item.Punteo ?? default(decimal);

				}
				return punteo;
			}
			else
			{
				return 0;
			}


		}


		public ActionResult ImprimirPdfNotas()
		{
			return View();
		}

		[Authorize(Roles = "Estudiante")]
		public ActionResult VerUnidadesNotas()
		{
			var consulta = from jornada in _context.TbUnidad
						   select new
						   {
							   jornada.Nombre,
							   jornada.CodUnidad
						   };
			foreach (var item in consulta)
			{
				TbUnidad jornadaaux = new TbUnidad();
				jornadaaux.Nombre = item.Nombre;
				jornadaaux.CodUnidad = item.CodUnidad;
				tbJornadas.Add(jornadaaux);
			}
			return View(tbJornadas);
		}

		[Authorize(Roles = "Estudiante")]
		[HttpPost]
		public ActionResult VerResultadosGenerales(string cod_orden)
		{
			int codUnidad12 = Int32.Parse(cod_orden);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			System.Diagnostics.Debug.WriteLine("USUARIO: " + userId);
			int CODIGODOC = 0;
			var doc = from Doc in _context.TbAlumno
					  where Doc.CodUsuario == int.Parse(userId)
					  select new
					  {
						  Doc.CodAlumno
					  };

			DateTime año = DateTime.Now;
			var año1 = año.Year;
			int año2 = año1;

			foreach (var item in doc)
			{
				CODIGODOC = item.CodAlumno;
			}

			var consulta = from cons1 in _context.TbAlumno
						   join cons2 in _context.TbClaseAlumno on cons1.CodAlumno equals cons2.CodAlumno
						   join cons3 in _context.TbCurso on cons1.CodGrado equals cons3.CodGrado
						   join cons4 in _context.TbClase on cons3.CodCurso equals cons4.CodCurso
						   join cons5 in _context.TbAño on cons4.CodAño equals cons5.CodAño
						   join cons6 in _context.TbInformacionPersonal on cons1.CodInformacionPersonal equals cons6.CodInformacionPersonal
						   join cons7 in _context.TbSeccion on cons4.CodSeccion equals cons7.CodSeccion
						   join cons8 in _context.TbUnidad on cons4.CodUnidad equals cons8.CodUnidad
						   join cons9 in _context.TbJornadas on cons4.CodJornada equals cons9.CodJornada
						   where cons1.CodAlumno == CODIGODOC && cons5.Numero == año2 && cons4.CodClase == cons2.CodClase && cons4.CodUnidad == codUnidad12
						   select new
						   {
							   cons4.CodClase,
							   Nombre = cons6.Nombre,
							   Apellido = cons6.Apellido,
							   Jornada = cons9.Nombre,
							   Seccion = cons7.Nombre,
							   Unidad = cons8.Nombre,
							   Curso = cons3.Nombre,
							   CodAlumno = cons1.CodAlumno
						   };


			List<int> listaClases = new List<int>();
			decimal promedio_aux = 0;
			decimal promedio_aux2 = 0;
			foreach (var item in consulta)
			{
				NotasGenerales ola = new NotasGenerales();
				listaClases.Add(item.CodClase);
				ola.Alumno = item.Nombre + " " + item.Apellido;
				ola.Clase = item.Curso + " " + item.Seccion + " " + item.Unidad + " " + item.Jornada;
				ola.CodAlumno = item.CodAlumno;
				listanotas.Add(ola);
			}

			for (int i = 0; i < listaClases.Count; i++)
			{
				System.Diagnostics.Debug.WriteLine("Clases: " + listaClases.ElementAt(i) + "Total: " + Punteo_Clases(CODIGODOC, listaClases.ElementAt(i), año2));
				listanotas.ElementAt(i).Punteo = Punteo_Clases(CODIGODOC, listaClases.ElementAt(i), año2);
				promedio_aux += Punteo_Clases(CODIGODOC, listaClases.ElementAt(i), año2);
				if (listanotas.ElementAt(i).Punteo >= 60)
				{
					listanotas.ElementAt(i).Aprobado = "Aprobado";
				}
				else
				{
					listanotas.ElementAt(i).Aprobado = "Reprobado";
				}
				promedio_aux2 = Math.Round((promedio_aux / listaClases.Count), 2);
				listanotas.ElementAt(i).Promedio = promedio_aux2;
			}
			ViewData["Lista_Alumnos4"] = listanotas.Count;
			if (ResultadoGeneralAlumno(listanotas).Equals("1"))
			{
				System.Diagnostics.Debug.WriteLine("El estudiante aprobo");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("El estudiante reprobo");
			}

			if (listanotas.Count() == 0)
			{
				return RedirectToAction("VerUnidadesNotas", "TbClases");
			}
			else
			{   //return View(listanotas);

				Document pdfDoc = new Document();
				pdfDoc.SetPageSize(PageSize.Letter.Rotate());
				pdfDoc.SetMargins(42f, 42f, 42f, 42f);
				//FileStream file = new FileStream("hola_mundo.pdf", FileMode.Create);
				MemoryStream ms = new MemoryStream();
				PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
				pdfDoc.AddAuthor("Eddie Macz");
				pdfDoc.AddTitle("Sistema de Notas");
				pdfDoc.Open();

				//Fuentes

				BaseFont _titulo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font titulo = new Font(_titulo, 18f, Font.BOLD, BaseColor.Black);

				BaseFont _subtitulo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font subtitulo = new Font(_subtitulo, 14f, Font.BOLD, BaseColor.Gray);

				BaseFont _parrafo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafo = new Font(_parrafo, 11f, Font.NORMAL, BaseColor.White);

				BaseFont _parrafo_cuerpo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafocuerpo = new Font(_parrafo_cuerpo, 10f, Font.NORMAL, BaseColor.Black);

				BaseFont _fondo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font fondo = new Font(_fondo, 18f, Font.BOLD, new BaseColor(255, 255, 255));

				//ENCABEZADO

				var tblenc = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100 };
				tblenc.AddCell(new PdfPCell(new Phrase("BOLETA DE CALIFICACIONES POR UNIDAD", titulo)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2 });
				tblenc.AddCell(new Phrase(" "));
				pdfDoc.Add(tblenc);

				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);

				tblenc = new PdfPTable(new float[] { 7f, 40f, 28f, 25f }) { WidthPercentage = 100 };
				iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "img1", "logo_sf.png"));
				float ancho = logo.Width;
				float alto = logo.Height;
				float proporcion = alto / ancho;

				logo.ScaleAbsolute(80, 80 * proporcion);
				tblenc.AddCell(new PdfPCell(logo) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 3, Colspan = 2 });
				tblenc.AddCell(new PdfPCell(new Phrase("COLEGIO SAGRADA FAMILIA", fondo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Alumno: " + listanotas.ElementAt(0).Alumno, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 1 });
				tblenc.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 1 });

				tblenc.AddCell(new PdfPCell(new Phrase("Telefono: +502 45459645", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 1 });
				tblenc.AddCell(new PdfPCell(new Phrase("San Pedro Carchá, A.V.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 1 });

				tblenc.AddCell(new PdfPCell(new Phrase("No.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Clase", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Punteo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Estado ", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });


				for (int i = 0; i < listanotas.Count(); i++)
				{
					tblenc.AddCell(new PdfPCell(new Phrase((i + 1).ToString(), parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(listanotas.ElementAt(i).Clase, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(listanotas.ElementAt(i).Punteo.ToString(), parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(listanotas.ElementAt(i).Aprobado, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				}

				tblenc.AddCell(new PdfPCell(new Phrase("Promedio ", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 3 });
				tblenc.AddCell(new PdfPCell(new Phrase(listanotas.ElementAt(listanotas.Count - 1).Promedio + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				pdfDoc.Add(tblenc);

				pdfWriter.Close();
				pdfDoc.Close();
				//ms.Seek(0, SeekOrigin.Begin);
				//var pdf = new FileStream("hola_mundo.pdf", FileMode.Open, FileAccess.Read);
				return File(ms.ToArray(), "application/pdf");

				//return new ViewAsPdf("VerResultadosGenerales", listanotas)
				//{
				//	PageSize = Rotativa.AspNetCore.Options.Size.Legal,
				//	PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				//};
			}
		}

		public decimal Punteo_Clases(int codalumno, int clase, int año)
		{
			decimal total = 0;
			var consulta2 = from cons1 in _context.TbAlumno
							join cons2 in _context.TbNota on cons1.CodAlumno equals cons2.CodAlumno
							join cons3 in _context.TbActividad on cons2.CodActividad equals cons3.CodActividad
							join cons4 in _context.TbClase on cons3.CodClase equals cons4.CodClase
							join cons5 in _context.TbCurso on cons4.CodCurso equals cons5.CodCurso
							join cons6 in _context.TbAño on cons4.CodAño equals cons6.CodAño
							where cons1.CodAlumno == codalumno && cons4.CodClase == clase && cons6.Numero == año
							select new
							{
								cons2.Punteo
							};
			if (consulta2.Count() > 0)
			{

				foreach (var item in consulta2)
				{
					total += item.Punteo ?? default(decimal);
				}
				return total;
			}
			else
			{
				return 0;
			}
		}

		public ActionResult ResultadosAño()
		{

			return View();
		}

		public string ResultadoGeneralAlumno(List<NotasGenerales> notasGenerales)
		{
			string Estado = "";
			for (int i = 0; i < notasGenerales.Count; i++)
			{
				if (notasGenerales.ElementAt(i).Aprobado.Equals("Aprobado"))
				{
					Estado = "Aprobado";
				}
				else
				{
					Estado = "Reprobado";
				}
			}

			if (Estado.Equals("Aprobado"))
			{
				return "1";
			}
			else
			{
				return "0";
			}
		}

		[Authorize(Roles = "Estudiante")]
		public ActionResult ResGeneral()
		{
			decimal Promedio = 0;
			decimal TotalAux = 0;
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string GradoAlumno = "";
			string NombreAlumno = "";
			int Cod_Al = 0;
			int Cod_Grado = 0;
			var doc = from Doc in _context.TbAlumno
					  join Doc1 in _context.TbInformacionPersonal on Doc.CodInformacionPersonal equals Doc1.CodInformacionPersonal
					  where Doc.CodUsuario == int.Parse(userId)
					  select new
					  {
						  Doc.CodAlumno,
						  Doc.CodGrado,
						  Nombre = Doc1.Nombre,
						  Apellido = Doc1.Apellido
					  };



			foreach (var item in doc)
			{
				Cod_Al = item.CodAlumno;
				Cod_Grado = item.CodGrado;
				NombreAlumno = item.Nombre + " " + item.Apellido;
			}

			var consulta2 = from con1 in _context.TbCurso
							join con3 in _context.TbGrado on con1.CodGrado equals con3.CodGrado
							join con4 in _context.TbNivel on con3.CodNivel equals con4.CodNivel
							where con1.CodGrado == Cod_Grado
							select new
							{
								con1.CodCurso,
								Grado = con3.Nombre,
								Nivel = con4.Nombre,
								Curso = con1.Nombre
							};

			foreach (var item in consulta2)
			{
				CodigoClases.Add(item.CodCurso);
				NombreClases.Add(item.Curso);
				GradoAlumno = item.Grado + " " + item.Nivel;
			}

			var consulta3 = from con1 in _context.TbUnidad
							select new
							{
								con1.CodUnidad
							};
			int TotalU = consulta3.Count();
			foreach (var item in consulta3)
			{
				CodigoUnidades.Add(item.CodUnidad);
			}

			for (int i = 0; i < CodigoClases.Count; i++)
			{
				TotalAux = 0;
				ResultadoGeneral lista1 = new ResultadoGeneral();
				lista1.Alumno = NombreAlumno;
				lista1.CodClase = CodigoClases.ElementAt(i);
				lista1.Clase = NombreClases.ElementAt(i);
				lista1.Punteos = new List<PunteoXUnidad>();
				for (int j = 0; j < CodigoUnidades.Count; j++)
				{
					PunteoXUnidad lista2 = new PunteoXUnidad();
					lista2.CodClase = CodigoUnidades.ElementAt(j);
					lista2.Punteo = ResultadoTotales(Cod_Al, CodigoClases.ElementAt(i), CodigoUnidades.ElementAt(j));
					TotalAux += lista2.Punteo;
					lista1.Punteos.Add(lista2);
				}
				Promedio = Math.Round(TotalAux / TotalU, 2);
				lista1.Total = Promedio;
				if (Promedio >= 60)
				{
					lista1.Estado = "Aprobado";
				}
				else
				{
					lista1.Estado = "Reprobado";
				}
				ResultadoGenerals.Add(lista1);
			}

			for (int i = 0; i < ResultadoGenerals.Count; i++)
			{
				System.Diagnostics.Debug.WriteLine("ALUMNO: " + ResultadoGenerals.ElementAt(i).Alumno + ", Clase:  " + ResultadoGenerals.ElementAt(i).Clase);
				for (int j = 0; j < ResultadoGenerals.ElementAt(i).Punteos.Count; j++)
				{
					System.Diagnostics.Debug.WriteLine("Unidad: " + ResultadoGenerals.ElementAt(i).Punteos.ElementAt(j).CodClase
							+ ",PUNTEO: " + ResultadoGenerals.ElementAt(i).Punteos.ElementAt(j).Punteo);
				}
			}

			if (ResultadoGenerals.Count() == 0)
			{
				return RedirectToAction("VerUnidadesNotas", "TbClases");
			}
			else
			{
				ViewData["Lista_Alumnos2"] = CodigoUnidades.Count;
				ResultadoGenerals.ElementAt(0).Contador1 = CodigoUnidades.Count;
				//return View(ResultadoGenerals);

				//GENERAR PDF
				Document pdfDoc = new Document();
				pdfDoc.SetPageSize(PageSize.Letter.Rotate());
				pdfDoc.SetMargins(45f, 45f, 45f, 45f);
				//FileStream file = new FileStream("hola_mundo.pdf", FileMode.Create);
				MemoryStream ms = new MemoryStream();
				PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, ms);
				pdfDoc.AddAuthor("Eddie Macz");
				pdfDoc.AddTitle("Sistema de Notas");
				pdfDoc.Open();

				//Fuentes

				BaseFont _titulo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font titulo = new Font(_titulo, 18f, Font.BOLD, BaseColor.Black);

				BaseFont _subtitulo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font subtitulo = new Font(_subtitulo, 14f, Font.BOLD, BaseColor.Gray);

				BaseFont _parrafo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafo = new Font(_parrafo, 9f, Font.NORMAL, BaseColor.White);

				BaseFont _parrafo_cuerpo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
				iTextSharp.text.Font parrafocuerpo = new Font(_parrafo_cuerpo, 8f, Font.NORMAL, BaseColor.Black);

				BaseFont _fondo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
				iTextSharp.text.Font fondo = new Font(_fondo, 18f, Font.BOLD, new BaseColor(255, 255, 255));

				//ENCABEZADO
				int num_act = TotalU;

				float[] columnas = new float[num_act + 5];
				columnas[0] = 5f;
				columnas[1] = 8f;
				columnas[2] = 20;
				columnas[columnas.Length - 2] = 10f;
				columnas[columnas.Length - 1] = 10f;
				for (int j = 3; j < columnas.Length - 2; j++)
				{
					columnas[j] = 10f;
				}

				var tblenc = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100 };
				tblenc.AddCell(new PdfPCell(new Phrase("BOLETA DE CALIFICACIONES", titulo)) { Border = 0, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER });
				tblenc.AddCell(new Phrase(" "));
				pdfDoc.Add(tblenc);

				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);
				pdfDoc.Add(Chunk.Newline);



				tblenc = new PdfPTable(columnas) { WidthPercentage = 100 };
				iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(_env.WebRootPath, "img1", "logo_sf.png"));
				float ancho = logo.Width;
				float alto = logo.Height;
				float proporcion = alto / ancho;

				logo.ScaleAbsolute(60, 60 * proporcion);
				tblenc.AddCell(new PdfPCell(logo) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Rowspan = 3, Colspan = 2 });
				tblenc.AddCell(new PdfPCell(new Phrase("COLEGIO SAGRADA FAMILIA", fondo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = columnas.Length - 2, Padding = 8 });

				tblenc.AddCell(new PdfPCell(new Phrase("Alumno: " + ResultadoGenerals.ElementAt(0).Alumno, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = columnas.Length - 2 });

				tblenc.AddCell(new PdfPCell(new Phrase("San Pedro Carchá, A.V.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = num_act + 1 });
				tblenc.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8, Colspan = 2 });

				tblenc.AddCell(new PdfPCell(new Phrase("No.", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Codigo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Nombre", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				for (int k = 1; k <= num_act; k++)
				{
					tblenc.AddCell(new PdfPCell(new Phrase("Unidad " + k, parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				}

				tblenc.AddCell(new PdfPCell(new Phrase("Punteo", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				tblenc.AddCell(new PdfPCell(new Phrase("Estado", parrafo)) { BorderColor = new BaseColor(242, 242, 242), BackgroundColor = new BaseColor(76, 175, 80), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });

				System.Diagnostics.Debug.WriteLine("LISTADO DE NOTAS GENERALES ");
				for (int l = 0; l < ResultadoGenerals.Count; l++)
				{
					tblenc.AddCell(new PdfPCell(new Phrase((l + 1).ToString(), parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase("CURSO -" + ResultadoGenerals.ElementAt(l).CodClase, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(ResultadoGenerals.ElementAt(l).Clase, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					for (int m = 0; m < ResultadoGenerals.ElementAt(l).Punteos.Count; m++)
					{
						//	System.Diagnostics.Debug.WriteLine("Actividad" + (m + 1) + ": " + alumno_notas.ElementAt(l).actividad_punteo.ElementAt(m).Punteo);
						tblenc.AddCell(new PdfPCell(new Phrase(ResultadoGenerals.ElementAt(l).Punteos.ElementAt(m).Punteo + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					}
					tblenc.AddCell(new PdfPCell(new Phrase(ResultadoGenerals.ElementAt(l).Total + "", parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
					tblenc.AddCell(new PdfPCell(new Phrase(ResultadoGenerals.ElementAt(l).Estado, parrafocuerpo)) { BorderColor = new BaseColor(242, 242, 242), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 8 });
				}




				pdfDoc.Add(tblenc);

				pdfWriter.Close();
				pdfDoc.Close();
				//ms.Seek(0, SeekOrigin.Begin);
				//var pdf = new FileStream("hola_mundo.pdf", FileMode.Open, FileAccess.Read);
				return File(ms.ToArray(), "application/pdf");

				//return new ViewAsPdf("ResGeneral", ResultadoGenerals)
				//{
				//	PageSize = Rotativa.AspNetCore.Options.Size.Legal,
				//	PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				//};
			}
		}

		[Authorize(Roles = "Administrador")]
		public async Task<ActionResult> CambiarAñoAsync()
		{
			decimal Promedio = 0;
			decimal TotalAux = 0;
			string NombreAlumno = "";
			int TotalClases = 0;
			var consulta = from tb1 in _context.TbAlumno
						   join tb2 in _context.TbInformacionPersonal on tb1.CodInformacionPersonal equals tb2.CodInformacionPersonal
						   select new
						   {
							   Grado = tb1.CodGrado,
							   CodAlumno = tb1.CodAlumno,
							   NombreA = tb2.Nombre,
							   ApellidoA = tb2.Apellido
						   };

			var consulta3 = from con1 in _context.TbUnidad
							select new
							{
								con1.CodUnidad
							};
			int TotalU = consulta3.Count();
			foreach (var item in consulta3)
			{
				CodigoUnidades1.Add(item.CodUnidad);
			}

			foreach (var item in consulta)
			{
				ListadoAlumnos lista1Al = new ListadoAlumnos();
				lista1Al.Nombre = item.NombreA + " " + item.ApellidoA;
				lista1Al.CodAlumno = item.CodAlumno;
				lista1Al.CodGrado = item.Grado;
				listados.Add(lista1Al);
			}

			for (int m = 0; m < listados.Count(); m++)
			{
				CodigoClases1 = new List<int>();
				NombreClases1 = new List<string>();
				var consulta2 = from con1 in _context.TbCurso
								join con3 in _context.TbGrado on con1.CodGrado equals con3.CodGrado
								join con4 in _context.TbNivel on con3.CodNivel equals con4.CodNivel
								where con1.CodGrado == listados.ElementAt(m).CodGrado
								select new
								{
									con1.CodCurso,
									Grado = con3.Nombre,
									Nivel = con4.Nombre,
									Curso = con1.Nombre
								};
				NombreAlumno = listados.ElementAt(m).Nombre;
				foreach (var item in consulta2)
				{
					CodigoClases1.Add(item.CodCurso);
					NombreClases1.Add(item.Curso);
				}

				TotalClases = CodigoClases1.Count();
				for (int i = 0; i < CodigoClases1.Count; i++)
				{
					TotalAux = 0;
					ResultadoGeneral lista1 = new ResultadoGeneral();
					lista1.Alumno = NombreAlumno;
					lista1.CodClase = CodigoClases1.ElementAt(i);
					lista1.Clase = NombreClases1.ElementAt(i);
					lista1.TotalClases = TotalClases;
					lista1.CodigoAlumno = listados.ElementAt(m).CodAlumno;
					lista1.Punteos = new List<PunteoXUnidad>();
					lista1.CodGrado = listados.ElementAt(m).CodGrado;
					for (int j = 0; j < CodigoUnidades1.Count; j++)
					{
						PunteoXUnidad lista2 = new PunteoXUnidad();
						lista2.CodClase = CodigoUnidades1.ElementAt(j);
						lista2.Punteo = ResultadoTotales(listados.ElementAt(m).CodAlumno, CodigoClases1.ElementAt(i), CodigoUnidades1.ElementAt(j));
						TotalAux += lista2.Punteo;
						lista1.Punteos.Add(lista2);
					}
					Promedio = Math.Round(TotalAux / TotalU, 2);
					lista1.Total = Promedio;
					if (Promedio >= 60)
					{
						lista1.Estado = "Aprobado";
					}
					else
					{
						lista1.Estado = "Reprobado";
					}
					ResultadoGenerals1.Add(lista1);
				}

			}
			bool Aprobado = true;
			int TotalClasesAux = 0;
			System.Diagnostics.Debug.WriteLine("Total de Registros: " + ResultadoGenerals1.Count);
			for (int i = 0; i < ResultadoGenerals1.Count; i++)
			{
				Aprobado = true;
				TotalClasesAux += ResultadoGenerals1.ElementAt(i).TotalClases;
				System.Diagnostics.Debug.WriteLine("ALUMNO: " + ResultadoGenerals1.ElementAt(i).Alumno + ", CodCLase: " + ResultadoGenerals1.ElementAt(i).CodClase
					+ ", Clase: " + ResultadoGenerals1.ElementAt(i).Clase + ", Total de Clases: " + ResultadoGenerals1.ElementAt(i).TotalClases
					+ ", Total: " + ResultadoGenerals1.ElementAt(i).Total);

				for (int j = i; j < TotalClasesAux; j++)
				{
					if (ResultadoGenerals1.ElementAt(j).Total < 60)
					{
						Aprobado = false;
					}
				}
				if (Aprobado)
				{
					System.Diagnostics.Debug.WriteLine("Aprobado-- Puede pasar de año");
					CambiarGrado(ResultadoGenerals1.ElementAt(i).CodigoAlumno, ResultadoGenerals1.ElementAt(i).CodGrado);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Perdio al menos una clase, no pasa de año.");
				}
				i = TotalClasesAux - 1;

			}



			return View();
		}

		public decimal ResultadoTotales(int codalumno, int codclase, int codunidad)
		{
			DateTime año = DateTime.Now;
			var año1 = año.Year;
			int año2 = año1;
			decimal TOTAL = 0;
			var consulta = from al in _context.TbNota
						   join al1 in _context.TbAlumno on al.CodAlumno equals al1.CodAlumno
						   join al2 in _context.TbActividad on al.CodActividad equals al2.CodActividad
						   join al3 in _context.TbClase on al2.CodClase equals al3.CodClase
						   join al4 in _context.TbUnidad on al3.CodUnidad equals al4.CodUnidad
						   join al5 in _context.TbAño on al3.CodAño equals al5.CodAño
						   where al1.CodAlumno == codalumno && al3.CodCurso == codclase && al3.CodUnidad == codunidad && al5.Numero == año2
						   select new
						   {
							   al.Punteo
						   };

			if (consulta.Count() > 0)
			{
				foreach (var item in consulta)
				{
					TOTAL += item.Punteo ?? default(decimal);
				}
				return TOTAL;
			}
			else
			{
				TOTAL = 0;
				return TOTAL;
			}



		}

		[Authorize(Roles = "Administrador")]
		public async void CambiarGrado(int codAlumno, int CodGrado)
		{
			var consulta = from ola in _context.TbGrado
						   where ola.CodGrado == CodGrado
						   select new
						   {
							   GradoSig = ola.CodGradoSiguiente
						   };

			int CodGrado1 = consulta.FirstOrDefault().GradoSig;

			var customer = _context.TbAlumno.Where(c => c.CodAlumno.Equals(codAlumno)).FirstOrDefault();
			customer.CodGrado = CodGrado1;
			System.Diagnostics.Debug.WriteLine("Grado Anterior: " + CodGrado + ", Grado Nuevo: " + customer.CodGrado);
			_context.TbAlumno.Update(customer);
			await _context.SaveChangesAsync();
		}

	}
}
