using Microsoft.AspNetCore.Mvc;

namespace SV20T1020242.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Nguyễn Việt Sơn",
                Birthday = new DateTime(2002, 01, 01),
                Salary = 500.25m
            };
            return View(model);
        }
        public IActionResult Save(Models.Person model)
        {
            return Json(model);
        }
    }
}
