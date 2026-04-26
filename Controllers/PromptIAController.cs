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

        // Ver prompt actual
        public async Task<IActionResult> Index()
        {
            var doc = await _db.Collection("prompt_ia").Document("config").GetSnapshotAsync();
            var prompt = doc.ConvertTo<PromptIA>();
            return View(prompt);
        }

        // Actualizar prompt
        [HttpPost]
        public async Task<IActionResult> Actualizar(PromptIA prompt)
        {
            await _db.Collection("prompt_ia").Document("config").SetAsync(prompt);
            return RedirectToAction("Index");
        }
    }
}