using Microsoft.AspNetCore.Mvc;
using SV20T1020242.BusinessLayers;
using SV20T1020242.DomainModels;
using System.Diagnostics;

namespace SV20T1020242.Web.Controllers
{
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;

        public IActionResult Index(int page = 1, string searchValue = "",
            int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int rowCount = 0;
            var data = ProductDataService.ListOfProducts(out rowCount, page, PAGE_SIZE, searchValue ?? "",
                categoryID, supplierID, minPrice, maxPrice);

            var model = new Models.ProductSearchResult()
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
            ViewBag.Title = "Bổ sung mặt hàng";
            var model = new Product()
            {
                ProductID = 0,
                IsSelling = true

            };
            ViewBag.IsEdit = false;
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.IsEdit = true;
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Product model, IFormFile? uploadPhoto = null)
        {
            if (string.IsNullOrWhiteSpace(model.ProductName))
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");

            if (string.IsNullOrWhiteSpace(model.ProductDescription))
                ModelState.AddModelError("ProductDescription", "Mô tả mặt hàng không được để trống");

            if (string.IsNullOrWhiteSpace(model.Unit))
                ModelState.AddModelError("Unit", "Đơn vị không được để trống");

            if (model.Price == 0)
                ModelState.AddModelError("Price", "Giá hàng không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                return View("Edit", model);
            }


            Debug.Write("Xuất xem: " + model.ProductID + "-" + model.ProductName + "-" +
                model.ProductDescription + "-" + model.Price + "-" + model.IsSelling + "-" + model.Photo
                + "-" + model.CategoryID + "-" + model.SupplierID);

            if (uploadPhoto != null)
            {
                Debug.WriteLine("Upload: " + uploadPhoto.FileName);
                string fileName = $"{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, path2: @"images\products", path3: fileName);
                Debug.WriteLine("Upload: " + uploadPhoto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                model.Photo = fileName;
            }

            if (model.ProductID == 0)
            { 
                int id = ProductDataService.AddProduct(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("ProductName", "Tên mặt hàng bị trùng!");
                    ViewBag.Title = "Bổ sung danh mục";
                    return View("Edit", model);
                }
            }

            else
            {
                Debug.Write(model);
                bool result = ProductDataService.UpdateProduct(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được mặt hàng, có thể tên mặc hàng bị trùng!");
                    ViewBag.Title = "Cập nhật thông tin mặt hàng";
                    return View("Edit", model);
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            ViewBag.Title = "Xóa mặt hàng";
            if (Request.Method == "POST")
            {
                bool result = ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var model = ProductDataService.GetProduct(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public IActionResult Photo(string id, string method, int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật ảnh cho mặt hàng";
                    return View();
                case "delete":
                    //TODO: xóa ảnh có mã là photoid trực tiếp
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");

            }

        }
        public IActionResult Attribute(string id, string method, int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính cho mặt hàng";
                    return View();
                case "delete":
                    //TODO: xóa ảnh có mã là photoid trực tiếp
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");

            }

        }
    }
}
