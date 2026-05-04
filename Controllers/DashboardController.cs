using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using TuProyecto.Models;

namespace TuProyecto.Controllers
{
    public class DashboardController : Controller
    {
        private readonly FirestoreDb _db;

        public DashboardController(FirestoreDb db)
        {
            _db = db;
        }

       public async Task<IActionResult> Index()
{
    var usuariosSnapshot = await _db.Collection("usuarios").GetSnapshotAsync();
    var usuarios = usuariosSnapshot.Documents
        .Select(d => d.ConvertTo<Usuario>())
        .ToList();

    ViewBag.TotalUsuarios = usuarios.Count;

    // Distribución por género
    ViewBag.PorGenero = usuarios
        .GroupBy(u => string.IsNullOrEmpty(u.Genero) ? "No especificado" : u.Genero)
        .ToDictionary(g => g.Key, g => g.Count());

    // Distribución por distrito
    ViewBag.PorDistrito = usuarios
        .GroupBy(u => string.IsNullOrEmpty(u.Distrito) ? "No especificado" : u.Distrito)
        .OrderByDescending(g => g.Count())
        .Take(5)
        .ToDictionary(g => g.Key, g => g.Count());

    // Distribución por edad
    ViewBag.PorEdad = new Dictionary<string, int>
    {
        { "Menos de 18", usuarios.Count(u => u.Edad < 18) },
        { "18-25", usuarios.Count(u => u.Edad >= 18 && u.Edad <= 25) },
        { "26-35", usuarios.Count(u => u.Edad >= 26 && u.Edad <= 35) },
        { "36-50", usuarios.Count(u => u.Edad >= 36 && u.Edad <= 50) },
        { "Más de 50", usuarios.Count(u => u.Edad > 50) }
    };

    // Total rutas
    var rutasSnapshot = await _db.Collection("rutas").GetSnapshotAsync();
    ViewBag.TotalRutas = rutasSnapshot.Count;

    // Total comentarios
    var comentariosSnapshot = await _db.Collection("comentarios").GetSnapshotAsync();
    ViewBag.TotalComentarios = comentariosSnapshot.Count;

    // Comentarios destacados
    var destacadosSnapshot = await _db.Collection("comentarios")
        .WhereEqualTo("destacado", true)
        .GetSnapshotAsync();
    ViewBag.TotalDestacados = destacadosSnapshot.Count;

    // Top rutas más buscadas
    var busquedasSnapshot = await _db.Collection("busquedas")
        .OrderByDescending("contador")
        .Limit(10)
        .GetSnapshotAsync();
    var topRutas = busquedasSnapshot.Documents
        .Select(d => new {
            Origen = d.GetValue<string>("origen"),
            Destino = d.GetValue<string>("destino"),
            Contador = d.GetValue<int>("contador")
        })
        .ToList();
    ViewBag.TopRutas = topRutas;

    return View();
}
    }
}