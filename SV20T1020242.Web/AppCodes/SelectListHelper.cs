using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020242.BusinessLayers;

namespace SV20T1020242.Web
{
    public class SelectListHelper
    {
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn Tỉnh/Thành --"
            });
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName,
                    Text = item.ProvinceName
                });
            }
            return list;
        }
        public static List<SelectListItem> Couster()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn khách hàng --"
            });

            int rowCount; // Khai báo biến để lưu số lượng hàng
            var customers = CommonDataService.ListOfCustomers(out rowCount); // Chuyển tham số rowCount vào phương thức

            if (customers != null)
            {
                foreach (var item in customers)
                {
                    list.Add(new SelectListItem()
                    {
                        Value = item.CustomerID.ToString(), // Giả sử CustomerID là một số nguyên và là giá trị bạn muốn sử dụng làm giá trị của mục chọn
                        Text = item.ContactName // Sử dụng ContactName làm văn bản của mục chọn
                    });
                }
            }
            return list;
        }
    }
}
