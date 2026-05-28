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
            // ── Usuarios ────────────────────────────────────────────
            var usuariosSnapshot = await _db.Collection("usuarios").GetSnapshotAsync();
            var usuarios = usuariosSnapshot.Documents
                .Select(d => d.ConvertTo<Usuario>())
                .ToList();

            ViewBag.TotalUsuarios = usuarios.Count;

            // Distribución por género
            ViewBag.PorGenero = usuarios
                .GroupBy(u => string.IsNullOrEmpty(u.Genero) ? "No especificado" : u.Genero)
                .ToDictionary(g => g.Key, g => g.Count());

            // Distribución por edad (rangos)
            ViewBag.PorEdad = new Dictionary<string, int>
            {
                { "<18",   usuarios.Count(u => u.Edad > 0 && u.Edad < 18) },
                { "18-25", usuarios.Count(u => u.Edad >= 18 && u.Edad <= 25) },
                { "26-35", usuarios.Count(u => u.Edad >= 26 && u.Edad <= 35) },
                { "36-50", usuarios.Count(u => u.Edad >= 36 && u.Edad <= 50) },
                { ">50",   usuarios.Count(u => u.Edad > 50) }
            };

            // Top 5 distritos de origen
            ViewBag.PorDistrito = usuarios
                .Where(u => !string.IsNullOrEmpty(u.Distrito))
                .GroupBy(u => u.Distrito)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Count());

            // ── Rutas ────────────────────────────────────────────────
            var rutasSnapshot = await _db.Collection("rutas").GetSnapshotAsync();
            var rutas = rutasSnapshot.Documents
                .Select(d => d.ConvertTo<Ruta>())
                .ToList();
            ViewBag.TotalRutas = rutas.Count;

            // ── Comentarios ──────────────────────────────────────────
            var comentariosSnapshot = await _db.Collection("comentarios").GetSnapshotAsync();
            var comentarios = comentariosSnapshot.Documents
                .Select(d => d.ConvertTo<Comentario>())
                .ToList();

            ViewBag.TotalComentarios = comentarios.Count;
            ViewBag.TotalDestacados  = comentarios.Count(c => c.Destacado);

            // Porcentaje de cobertura (destacados sobre total)
            int pctCobertura = comentarios.Count > 0
                ? (int)Math.Round(comentarios.Count(c => c.Destacado) * 100.0 / comentarios.Count)
                : 0;
            ViewBag.PctCobertura = pctCobertura;

            // Rutas con más comentarios (para sección Rutas y Comunidad)
            var comentariosPorRuta = comentarios
                .GroupBy(c => c.RutaId)
                .Select(g =>
                {
                    var ruta = rutas.FirstOrDefault(r => r.Id == g.Key);
                    return new
                    {
                        RutaId   = g.Key,
                        NombreRuta = ruta != null
                            ? (string.IsNullOrEmpty(ruta.Nombre) ? ruta.Codigo : ruta.Nombre)
                            : g.Key,
                        CodigoRuta = ruta?.Codigo ?? "",
                        Total      = g.Count(),
                        Negativos = g.Count(c => !c.Destacado),   // no destacados = negativos/normales
                        Positivos = g.Count(c =>  c.Destacado)
                    };
                })
                .OrderByDescending(x => x.Total)
                .Take(4)
                .ToList<dynamic>();
            ViewBag.ComentariosPorRuta = comentariosPorRuta;

            // ── Búsquedas ────────────────────────────────────────────
           var busquedasSnapshot = await _db.Collection("busquedas_global")
    .OrderByDescending("fecha")
    .Limit(200)
    .GetSnapshotAsync();

          var topRutas = busquedasSnapshot.Documents
    .Select(d => new
    {
        Origen   = d.ContainsField("origen")  ? d.GetValue<string>("origen")  : "—",
        Destino  = d.ContainsField("destino") ? d.GetValue<string>("destino") : "—",
        Contador = 1  // cada documento es una búsqueda
    })
    .GroupBy(x => new { x.Origen, x.Destino })
    .Select(g => new
    {
        Origen   = g.Key.Origen,
        Destino  = g.Key.Destino,
        Contador = g.Count()
    })
    .OrderByDescending(x => x.Contador)
    .Take(10)
    .ToList();

ViewBag.TopRutas       = topRutas;
ViewBag.TotalBusquedas = topRutas.Sum(r => r.Contador);

            return View();
        }
    }
}