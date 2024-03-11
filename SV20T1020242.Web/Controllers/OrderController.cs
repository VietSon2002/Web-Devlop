using Microsoft.AspNetCore.Mvc;

namespace SV20T1020242.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id = 0)
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult EditDetail(int id = 0, int idproductId = 0)
        {
            return View();
        }
        public IActionResult Shipping(int id = 0)
        {
            return View();
        }
    }
}
