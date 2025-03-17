using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CMS.Models;
using System.Linq;
using BCrypt.Net;

namespace CMS.Controllers
{
    [Route("Login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public LoginController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // ========== ĐĂNG NHẬP ==========
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm] UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = _dbc.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng!");
                    return View(model);
                }

                // ✅ Lưu thông tin người dùng vào Session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetInt32("RoleId", user.RoleId);
                Console.WriteLine($"RoleId: {user.RoleId}");
                // ✅ Nếu có trang trước đó, quay lại trang đó
                var returnUrl = HttpContext.Request.Query["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống, vui lòng thử lại sau!");
                Console.WriteLine(ex.Message); // Log lỗi (hoặc sử dụng Logger)
                return View(model);
            }
        }

        // ========== ĐĂNG KÝ ==========
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public IActionResult Register([FromForm] UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra email đã tồn tại chưa
            if (_dbc.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email đã tồn tại!");
                return View(model);
            }

            // Kiểm tra số điện thoại đã tồn tại chưa (nếu cần)
            if (_dbc.Users.Any(u => u.PhoneNum == model.PhoneNum))
            {
                ModelState.AddModelError("", "Số điện thoại đã tồn tại!");
                return View(model);
            }

            // Mã hóa mật khẩu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newUser = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = hashedPassword,
                PhoneNum = model.PhoneNum, // ✅ Thêm số điện thoại
                RoleId = 3 // ✅ Mặc định Customer
            };

            _dbc.Users.Add(newUser);
            _dbc.SaveChanges();

            return RedirectToAction("Login");
        }


        // ========== ĐĂNG XUẤT ==========
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa tất cả session
            return RedirectToAction("Login", "Login");
        }
    }
}
