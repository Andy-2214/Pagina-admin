using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class PromptIAController : Controller
    {
        private readonly FirestoreDb _db;

        public PromptIAController(FirestoreDb db)
        {
            _db = db;
        }

        // Ver prompt actual e historial
        public async Task<IActionResult> Index()
        {
            var doc = await _db.Collection("prompt_ia").Document("config").GetSnapshotAsync();
            var prompt = doc.ConvertTo<PromptIA>();

            // Cargar historial
            var historialSnapshot = await _db.Collection("prompt_ia_historial")
                .OrderByDescending("fechaModificacion")
                .Limit(10)
                .GetSnapshotAsync();
            var historial = historialSnapshot.Documents
                .Select(d => d.ConvertTo<PromptIA>())
                .ToList();

            ViewBag.Historial = historial;
            return View(prompt);
        }

        // Actualizar prompt
        [HttpPost]
        public async Task<IActionResult> Actualizar(PromptIA prompt)
        {
            // Guardar versión anterior en historial
            var docActual = await _db.Collection("prompt_ia").Document("config").GetSnapshotAsync();
            if (docActual.Exists)
            {
                var promptActual = docActual.ConvertTo<PromptIA>();
                promptActual.FechaModificacion = DateTime.UtcNow;
                await _db.Collection("prompt_ia_historial").AddAsync(promptActual);
            }

            // Guardar nuevo prompt
            prompt.FechaModificacion = DateTime.UtcNow;
            prompt.Autor = HttpContext.Session.GetString("AdminEmail") ?? "Admin";
            await _db.Collection("prompt_ia").Document("config").SetAsync(prompt);

            return RedirectToAction("Index");
        }

        // Restaurar versión anterior
        public async Task<IActionResult> Restaurar(string id)
        {
            var doc = await _db.Collection("prompt_ia_historial").Document(id).GetSnapshotAsync();
            var prompt = doc.ConvertTo<PromptIA>();
            prompt.FechaModificacion = DateTime.UtcNow;
            prompt.Autor = HttpContext.Session.GetString("AdminEmail") ?? "Admin";
            await _db.Collection("prompt_ia").Document("config").SetAsync(prompt);
            return RedirectToAction("Index");
        }
    }
}