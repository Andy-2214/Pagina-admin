using Google.Cloud.Firestore;

namespace TuProyecto.Models
{
    [FirestoreData]
    public class Ruta
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("codigo")]
        public string Codigo { get; set; }

        [FirestoreProperty("color")]
        public string Color { get; set; }

        [FirestoreProperty("nombre")]
        public string Nombre { get; set; }

        [FirestoreProperty("avenidas")]
        public string Avenidas { get; set; }
    }
}