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

        // Lista con buscador
        public async Task<IActionResult> Index(string buscar = "")
        {
            var snapshot = await _db.Collection("usuarios").GetSnapshotAsync();
            var usuarios = snapshot.Documents
                .Select(d => d.ConvertTo<Usuario>())
                .ToList();

            if (!string.IsNullOrEmpty(buscar))
            {
                usuarios = usuarios.Where(u =>
                    u.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            ViewBag.Buscar = buscar;
            return View(usuarios);
        }

        // Ver perfil detallado
        public async Task<IActionResult> Perfil(string id)
        {
            var doc = await _db.Collection("usuarios").Document(id).GetSnapshotAsync();
            var usuario = doc.ConvertTo<Usuario>();

            // Cargar comentarios del usuario
            var comentariosSnapshot = await _db.Collection("comentarios")
                .WhereEqualTo("usuarioId", id)
                .GetSnapshotAsync();
            var comentarios = comentariosSnapshot.Documents
                .Select(d => d.ConvertTo<Comentario>())
                .ToList();

            ViewBag.Comentarios = comentarios;
            return View(usuario);
        }

        // Eliminar usuario
        public async Task<IActionResult> Eliminar(string id)
        {
            await _db.Collection("usuarios").Document(id).DeleteAsync();
            return RedirectToAction("Index");
        }
    }
}