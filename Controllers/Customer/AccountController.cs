using Microsoft.AspNetCore.Mvc;
using CMS.Models;

namespace CMS.Controllers.Customer
{
    public class AccountController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public AccountController(CinemaDbContext dbc)
        {
            _dbc = dbc;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home"); // Nếu chưa có UserId trong session, chuyển về trang chủ
            }

            var customer = _dbc.Users.FirstOrDefault(u => u.UserId == userId);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Update(User updatedUser)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var customer = _dbc.Users.FirstOrDefault(u => u.UserId == userId);
            if (customer == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khách hàng
            customer.UserName = updatedUser.UserName;
            customer.Email = updatedUser.Email;
            customer.PhoneNum = updatedUser.PhoneNum;

            _dbc.Users.Update(customer);
            _dbc.SaveChanges();

            ViewBag.Message = "Cập nhật thông tin thành công!";
            return View("Index", customer);
        }
    }
}
