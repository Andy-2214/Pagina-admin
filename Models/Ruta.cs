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