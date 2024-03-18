using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020242.DomainModels
{
    /// <summary>
    /// Thông tin tài khoảng trong CSDL
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// ID tài khoảng
        /// </summary>
        public string UserID { get; set; } = "";
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// Tên đầy đảu
        /// </summary>
        public string FullName { get; set; } = "";
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        public string Photo { get; set; } = "";
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; } = "";
        /// <summary>
        /// CHuỗi các quyền của tài khoảng, phân cách bởi dấu phẩy
        /// </summary>
        public string RoleNames { get; set; } = "";
    }
}
