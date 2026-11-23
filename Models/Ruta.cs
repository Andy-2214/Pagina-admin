using Google.Cloud.Firestore;
using System.Text.Json;

namespace TuProyecto.Models
{
    [FirestoreData]
    public class Ruta
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("codigo")]
        public string Codigo { get; set; } = "";

        [FirestoreProperty("color")]
        public string Color { get; set; }

        [FirestoreProperty("nombre")]
        public string Nombre { get; set; } = "";

        [FirestoreProperty("etiqueta")]
        public string Etiqueta { get; set; } = "";

        [FirestoreProperty("avenidas")]
        public string Avenidas { get; set; } = "";

        [FirestoreProperty("avenidaVuelta")]
        public string AvenidaVuelta { get; set; } = "";

        [FirestoreProperty("empresa")]
        public string Empresa { get; set; } = "";

        // ── Campos que usa la app Android ────────────────────────────────────
        // Deben existir en el modelo para que el formulario los conserve al editar.
        // La app descarta una ruta cuya cuenca no existe en la coleccion "cuencas".
        [FirestoreProperty("cuenca")]
        public string Cuenca { get; set; } = "";

        [FirestoreProperty("codigoCombi")]
        public string CodigoCombi { get; set; } = "";

        [FirestoreProperty("frecuencia")]
        public string Frecuencia { get; set; } = "";

        // La app compara este numero para saber si una ruta cambio.
        // No esta en los formularios: Editar lo sube en 1 al guardar.
        [FirestoreProperty("version")]
        public long Version { get; set; } = 1;

        // ── Coordenadas IDA ───────────────────────────────────────────────
        [FirestoreProperty("coordenadas")]
        public List<Dictionary<string, double>> Coordenadas { get; set; } = new();

        public string CoordenadasJson
        {
            get => JsonSerializer.Serialize(Coordenadas);
            set
            {
                if (!string.IsNullOrEmpty(value))
                    Coordenadas = JsonSerializer.Deserialize<List<Dictionary<string, double>>>(value) ?? new();
            }
        }

        // ── Coordenadas VUELTA ─────────────────────────────────────────────
        [FirestoreProperty("coordenadasVuelta")]
        public List<Dictionary<string, double>> CoordenadasVuelta { get; set; } = new();

        public string CoordenadasVueltaJson
        {
            get => JsonSerializer.Serialize(CoordenadasVuelta);
            set
            {
                if (!string.IsNullOrEmpty(value))
                    CoordenadasVuelta = JsonSerializer.Deserialize<List<Dictionary<string, double>>>(value) ?? new();
            }
        }
    }
}