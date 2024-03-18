using SV20T1020242.DomainModels;

namespace SV20T1020242.Web.Models
{
    /// <summary>
    /// Đại diện cho dữ liệu sử dụng cho chức năng hiển thị chi tiết của đơn hàng (Order/Details).
    /// </summary>
    public class OrderDetailModel
    {
        /// <summary>
        /// Thông tin đơn hàng.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Danh sách chi tiết của đơn hàng.
        /// </summary>
        public List<OrderDetail> Details { get; set; }
        public string Status { get; set; }
    }
}
