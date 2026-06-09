using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class RutaController : Controller
    {
        private readonly FirestoreDb _db;

        public RutaController(FirestoreDb db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string buscar = "")
        {
            var snapshot = await _db.Collection("rutas").GetSnapshotAsync();
            var rutas = snapshot.Documents
                .Select(d => d.ConvertTo<Ruta>())
                .ToList();

            if (!string.IsNullOrEmpty(buscar))
                rutas = rutas.Where(r =>
                    r.Codigo.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    r.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    r.Empresa.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            ViewBag.Buscar = buscar;
            return View(rutas);
        }

        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(Ruta ruta)
        {
            // CoordenadasJson y CoordenadasVueltaJson ya fueron procesados
            // por los setters del modelo al hacer el binding del form.
            // Solo hay que guardar el objeto directamente.
            await _db.Collection("rutas").AddAsync(ruta);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(string id)
        {
            var doc = await _db.Collection("rutas").Document(id).GetSnapshotAsync();
            var ruta = doc.ConvertTo<Ruta>();
            return View(ruta);
        }

       [HttpPost]
public async Task<IActionResult> Editar(string id, Ruta ruta)
{
    // debug
    var json = Request.Form["CoordenadasJson"].ToString();
    Console.WriteLine("CoordenadasJson recibido: " + json);
    Console.WriteLine("Coordenadas count: " + ruta.Coordenadas.Count);
    
    await _db.Collection("rutas").Document(id).SetAsync(ruta);
    return RedirectToAction("Index");
}

        public async Task<IActionResult> Eliminar(string id)
        {
            await _db.Collection("rutas").Document(id).DeleteAsync();
            return RedirectToAction("Index");
        }
    }
}