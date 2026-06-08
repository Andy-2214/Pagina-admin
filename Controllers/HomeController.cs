using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PaginaAdminSITP.Models;
using System.Diagnostics;

namespace PaginaAdminSITP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FirestoreDb _db;

    public HomeController(ILogger<HomeController> logger, FirestoreDb db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("AdminEmail") == null)
            return RedirectToAction("Login", "Auth");

        var usuariosSnap = await _db.Collection("usuarios").GetSnapshotAsync();
        var rutasSnap    = await _db.Collection("rutas").GetSnapshotAsync();

        ViewBag.TotalUsuarios = usuariosSnap.Count;
        ViewBag.TotalRutas    = rutasSnap.Count;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}