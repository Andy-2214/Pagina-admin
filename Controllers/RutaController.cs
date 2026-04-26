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

        // Lista de rutas
        public async Task<IActionResult> Index()
        {
            var snapshot = await _db.Collection("rutas").GetSnapshotAsync();
            var rutas = snapshot.Documents
                .Select(d => d.ConvertTo<Ruta>())
                .ToList();
            return View(rutas);
        }

        // Formulario agregar
        public IActionResult Crear()
        {
            return View();
        }

        // Guardar ruta
        [HttpPost]
        public async Task<IActionResult> Crear(Ruta ruta)
        {
            await _db.Collection("rutas").AddAsync(ruta);
            return RedirectToAction("Index");
        }

        // Eliminar ruta
        public async Task<IActionResult> Eliminar(string id)
        {
            await _db.Collection("rutas").Document(id).DeleteAsync();
            return RedirectToAction("Index");
        }


        // Mostrar formulario de editar
public async Task<IActionResult> Editar(string id)
{
    var doc = await _db.Collection("rutas").Document(id).GetSnapshotAsync();
    var ruta = doc.ConvertTo<Ruta>();
    return View(ruta);
}

// Guardar edicion
[HttpPost]
public async Task<IActionResult> Editar(string id, Ruta ruta)
{
    await _db.Collection("rutas").Document(id).SetAsync(ruta);
    return RedirectToAction("Index");
}
    }
}