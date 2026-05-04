using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Inicializar Firebase
FirebaseApp.Create(new AppOptions {
    Credential = GoogleCredential.FromFile("firebase-credentials.json")
});

Environment.SetEnvironmentVariable(
    "GOOGLE_APPLICATION_CREDENTIALS",
    "firebase-credentials.json"
);

// Inicializar Firestore y registrar como servicio
var db = FirestoreDb.Create("sistema-de-transporte-7ff2b"); // ← aquí va tu ID
builder.Services.AddSingleton(db);

// Servicios MVC
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();