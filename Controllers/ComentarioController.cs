using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class ComentarioController : Controller
    {
        private readonly FirestoreDb _db;

        public ComentarioController(FirestoreDb db)
        {
            _db = db;
        }

        // Lista con filtros
        public async Task<IActionResult> Index(string filtroRuta = "", string filtroEstado = "")
        {
            var snapshot = await _db.Collection("comentarios").GetSnapshotAsync();
            var comentarios = snapshot.Documents
                .Select(d => d.ConvertTo<Comentario>())
                .OrderByDescending(c => c.Destacado)
                .ThenByDescending(c => c.Fecha)
                .ToList();

            if (!string.IsNullOrEmpty(filtroRuta))
                comentarios = comentarios.Where(c => c.RutaId == filtroRuta).ToList();

            if (filtroEstado == "destacado")
                comentarios = comentarios.Where(c => c.Destacado).ToList();

            // Cargar rutas para el filtro
            var rutasSnapshot = await _db.Collection("rutas").GetSnapshotAsync();
            var rutas = rutasSnapshot.Documents.Select(d => d.ConvertTo<Ruta>()).ToList();

            ViewBag.Rutas = rutas;
            ViewBag.FiltroRuta = filtroRuta;
            ViewBag.FiltroEstado = filtroEstado;

            return View(comentarios);
        }

        // Eliminar comentario
        public async Task<IActionResult> Eliminar(string id)
        {
            await _db.Collection("comentarios").Document(id).DeleteAsync();
            return RedirectToAction("Index");
        }

        // Destacar comentario
        public async Task<IActionResult> Destacar(string id, bool destacado)
        {
            await _db.Collection("comentarios").Document(id).UpdateAsync(
                new Dictionary<string, object> { { "destacado", !destacado } }
            );
            return RedirectToAction("Index");
        }
    }
}