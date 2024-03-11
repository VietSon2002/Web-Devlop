using Microsoft.AspNetCore.Mvc;
using SV20T1020242.BusinessLayers;
using SV20T1020242.DomainModels;


namespace SV20T1020242.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 10;

        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            var model = new Models.CategorySearchResult()
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
            ViewBag.Title = "Bổ sung danh mục";
            var model = new Category()
            {
                CategoryID = 0
            };

            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin danh mục";
            var model = CommonDataService.GetCategory(id);

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Title = "Xóa danh mục";

            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCategory(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);

        }

        [HttpPost]
        public IActionResult Save(Category model)
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
                ModelState.AddModelError("CategoryName", "Tên danh mục không đươc để trống");
            if (string.IsNullOrWhiteSpace(model.Description))
                ModelState.AddModelError("Description", "Mô tả không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung mô tả" : "Cập nhật mô tả";
                return View("Edit", model);
            }

            if (model.CategoryID == 0)
            { 
                int id = CommonDataService.AddCategory(model);
                if (id <= 0) 
                {
                    ModelState.AddModelError("CategoryName", "Tên danh mục bị trùng bị trùng!");
                    ViewBag.Title = "Bổ sung shipper";
                    return View("Edit", model);
                }
            }

            else
            {
                bool result = CommonDataService.UpdateCategory(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được danh mục, có thể tên danh mục bị trùng!");
                    ViewBag.Title = "Cập nhật thông tin danh mục";
                    return View("Edit", model);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
