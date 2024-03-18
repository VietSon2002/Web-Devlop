using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020242.BusinessLayers;

namespace SV20T1020242.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = " ", string password = " ")
        {
            ViewBag.Username = username;
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Nhập đủ tên và mật khẩu");
                return View();
            }
            //Kiểm tra thông tin đăng nhập có hợp lệ không?
            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)   
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }
            // Đăng nhập thành công, tạo dữ liệu để lưu cookie
            WebUserData userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,  
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = " ",
                Roles = userAccount.RoleNames.Split(',').ToList(),
            };
            await HttpContext.SignInAsync(userData.CreatePrincipal());

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            // Thực hiện xác thực người dùng và kiểm tra mật khẩu hiện tại
            // Nếu mật khẩu hiện tại không chính xác, trả về thông báo lỗi

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới có khớp nhau không
            // Nếu không khớp, trả về thông báo lỗi

            // Thực hiện thay đổi mật khẩu trong cơ sở dữ liệu

            // Trả về trang thông báo thành công hoặc chuyển hướng đến trang khác

            return View();
        }
        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
