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

        [FirestoreProperty("avenidas")]
        public string Avenidas { get; set; } = "";

        [FirestoreProperty("coordenadas")]
        public List<Dictionary<string, double>> Coordenadas { get; set; } = new();

        // Para manejar el JSON del formulario
        public string CoordenadasJson
        {
            get => JsonSerializer.Serialize(Coordenadas);
            set
            {
                if (!string.IsNullOrEmpty(value))
                    Coordenadas = JsonSerializer.Deserialize<List<Dictionary<string, double>>>(value) ?? new();
            }
        }

 [FirestoreProperty("empresa")]
public string Empresa { get; set; } = "";
    }
}