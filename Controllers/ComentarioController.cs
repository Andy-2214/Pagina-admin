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

        // Lista de comentarios
        public async Task<IActionResult> Index()
        {
            var snapshot = await _db.Collection("comentarios").GetSnapshotAsync();
            var comentarios = snapshot.Documents
                .Select(d => d.ConvertTo<Comentario>())
                .ToList();
            return View(comentarios);
        }

        // Eliminar comentario
        public async Task<IActionResult> Eliminar(string id)
        {
            await _db.Collection("comentarios").Document(id).DeleteAsync();
            return RedirectToAction("Index");
        }
    }
}