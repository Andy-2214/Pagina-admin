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

        // Documento contador que lee la app Android: si "rutas" cambia, la app vuelve a
        // descargar rutas y cuencas. Se sube en el MISMO batch que el cambio, asi nunca
        // queda una ruta cambiada sin version nueva (ni al reves).
        // Set + MergeAll (en vez de Update) para que no falle si el documento no existe.
        private void SubirVersionRutas(WriteBatch batch)
        {
            var meta = _db.Collection("metadatos").Document("version");
            batch.Set(meta, new Dictionary<string, object>
            {
                { "rutas", FieldValue.Increment(1) },
                { "actualizadoEn", FieldValue.ServerTimestamp }
            }, SetOptions.MergeAll);
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
            var batch = _db.StartBatch();
            batch.Set(_db.Collection("rutas").Document(), ruta);
            SubirVersionRutas(batch);
            await batch.CommitAsync();
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
    
    var docRef = _db.Collection("rutas").Document(id);
    var actual = await docRef.GetSnapshotAsync();
    if (!actual.Exists)
        return NotFound();

    // La app compara la version de cada ruta para saber si cambio.
    // Si el documento no tiene version, la app asume 1: se parte de ahi.
    long versionActual = actual.TryGetValue("version", out long v) ? v : 1L;
    ruta.Version = versionActual + 1;

    // MergeAll: solo escribe los campos del modelo y NO borra los demas
    // campos que pueda tener el documento.
    var batch = _db.StartBatch();
    batch.Set(docRef, ruta, SetOptions.MergeAll);
    SubirVersionRutas(batch);
    await batch.CommitAsync();
    return RedirectToAction("Index");
}

        public async Task<IActionResult> Eliminar(string id)
        {
            var batch = _db.StartBatch();
            batch.Delete(_db.Collection("rutas").Document(id));
            SubirVersionRutas(batch);
            await batch.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}