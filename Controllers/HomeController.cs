using LeadSpot.Data;
using LeadSpot.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeadSpot.Controllers;

public class HomeController : Controller
{
    private readonly DataAccess _dataAccess;
    
    public HomeController(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public IActionResult Index()
    {
        try
        {
            var users = _dataAccess.GetUsers();
            return View(users);

        }
        catch (Exception e)
        {
            TempData["Error"] = "Ocorreu um erro na criação de usuário!";
            return View();
        }
    }
}