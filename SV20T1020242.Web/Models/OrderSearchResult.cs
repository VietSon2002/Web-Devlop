using SV20T1020242.DomainModels;

namespace SV20T1020242.Web.Models
{
    /// <summary>
    /// Đại diện cho kết quả tìm kiếm đơn hàng.
    /// </summary>
    public class OrderSearchResult : BasePaginationResult
    {
        /// <summary>
        /// Trạng thái của đơn hàng.
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// Khoảng thời gian tìm kiếm.
        /// </summary>
        public string TimeRange { get; set; } = "";

        /// <summary>
        /// Danh sách đơn hàng tìm thấy.
        /// </summary>
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
