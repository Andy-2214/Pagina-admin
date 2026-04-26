using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly FirestoreDb _db;

        public UsuarioController(FirestoreDb db)
        {
            _db = db;
        }

        // Lista de usuarios
        public async Task<IActionResult> Index()
        {
            var snapshot = await _db.Collection("usuarios").GetSnapshotAsync();
            var usuarios = snapshot.Documents
                .Select(d => d.ConvertTo<Usuario>())
                .ToList();
            return View(usuarios);
        }

        // Eliminar usuario
        public async Task<IActionResult> Eliminar(string id)
        {
            await _db.Collection("usuarios").Document(id).DeleteAsync();
            return RedirectToAction("Index");
        }
    }
}