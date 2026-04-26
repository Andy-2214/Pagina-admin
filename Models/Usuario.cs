using Google.Cloud.Firestore;

namespace TuProyecto.Models
{
    [FirestoreData]
    public class Usuario
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("nombre")]
        public string Nombre { get; set; }

        [FirestoreProperty("email")]
        public string Email { get; set; }

        [FirestoreProperty("fechaRegistro")]
        public DateTime FechaRegistro { get; set; }
    }
}