using Microsoft.AspNetCore.Mvc;
using SV20T1020242.BusinessLayers;
using SV20T1020242.DomainModels;
using System.Diagnostics;

namespace SV20T1020242.Web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 9;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.EmployeeSearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            var model = new Employee()
            {
                EmployeeID = 0,
                BirthDate = new DateTime(2000, 1, 1),
                Photo = "nophoto.png",
                IsWorking = true
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (model.Photo == "") model.Photo = "nophoto.png";
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee model, string birthDateInput = "", IFormFile? uploadPhoto = null)
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError("FullName", "Tên nhân viên không đươc để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Số điện thoại không đươc để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                return View("Edit", model);
            }

            DateTime? d = birthDateInput.ToDateTime();
            if (d.HasValue)
            {
                model.BirthDate = d.Value;
            }
            if (uploadPhoto != null)
            {
                Debug.WriteLine("Upload: " + uploadPhoto.FileName);
                string fileName = $"{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, path2: @"images\employees", path3: fileName);
                Debug.WriteLine("Upload: " + uploadPhoto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                model.Photo = fileName;
            }

            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại bị trùng!");
                    ViewBag.Title = "Bổ sung nhân viên";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được nhân viên, có thể số điện thoại bị trùng!");
                    ViewBag.Title = "Cập nhật thông tin nhân viên";
                    return View("Edit", model);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
