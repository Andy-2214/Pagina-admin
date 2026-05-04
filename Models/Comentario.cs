using Google.Cloud.Firestore;

namespace TuProyecto.Models
{
    [FirestoreData]
    public class Comentario
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("rutaId")]
        public string RutaId { get; set; }

        [FirestoreProperty("texto")]
        public string Texto { get; set; }

        [FirestoreProperty("usuarioId")]
        public string UsuarioId { get; set; }

        [FirestoreProperty("fecha")]
        public DateTime Fecha { get; set; }

        [FirestoreProperty("destacado")]
        public bool Destacado { get; set; }
    }
}