using Google.Cloud.Firestore;

namespace TuProyecto.Models
{
    [FirestoreData]
    public class PromptIA
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("texto")]
        public string Texto { get; set; }
    }
}