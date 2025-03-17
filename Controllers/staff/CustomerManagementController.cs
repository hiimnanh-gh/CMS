using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMS.Models;

namespace CMS.Controllers.Admin
{
    public class CustomerManagementController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public CustomerManagementController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // ========== HIỂN THỊ DANH SÁCH KHÁCH HÀNG ==========
        public IActionResult Index(string search, int page = 1)
        {
            int pageSize = 5; // Số khách hàng mỗi trang
            var query = _dbc.Users
                .Where(u => u.RoleId == 3) // Lọc chỉ lấy khách hàng
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
                ViewData["SearchQuery"] = search;
            }

            var custList = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewData["CurrentPage"] = page;

            return View(custList);
        }


        // ========== THÊM KHÁCH HÀNG ==========
        // Hiển thị form thêm khách hàng
        [HttpGet("CreateCustomer")]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        // Xử lý thêm khách hàng
        [HttpPost("CreateCustomer")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(CustomerCreateModel model)
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

            // Kiểm tra số điện thoại đã tồn tại chưa
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
                PhoneNum = model.PhoneNum,
                RoleId = 3 // ✅ Mặc định Customer
            };

            _dbc.Users.Add(newUser);
            _dbc.SaveChanges();

            return RedirectToAction("Index");
        }





        // ========== CHỈNH SỬA KHÁCH HÀNG ==========
        // ========== HIỂN THỊ FORM CHỈNH SỬA ==========
        [HttpGet("EditCustomer/{id}")]
        public IActionResult EditCustomer(int id)
        {
            var customer = _dbc.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Roles = _dbc.Roles.ToList(); // ✅ Lấy danh sách Role để hiển thị

            return View(customer);
        }

        [HttpPost("EditCustomer/{id}")]
        public IActionResult EditCustomer(int id, User customerData)
        {
            var existingCustomer = _dbc.Users.FirstOrDefault(u => u.UserId == id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            // ✅ Cập nhật thông tin
            existingCustomer.UserName = customerData.UserName;
            existingCustomer.Email = customerData.Email;
            existingCustomer.PhoneNum = customerData.PhoneNum;

            // ✅ Nếu RoleId được chọn khác với RoleId hiện tại => cập nhật
            if (customerData.RoleId != existingCustomer.RoleId)
            {
                existingCustomer.RoleId = customerData.RoleId;
            }

            _dbc.Users.Update(existingCustomer);
            _dbc.SaveChanges();

            return RedirectToAction("Index", "CustomerManagement");
        }



        // ========== HIỂN THỊ FORM XÁC NHẬN XÓA KHÁCH HÀNG ==========
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cust = _dbc.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == id && u.Role.RoleName == "Customer");

            if (cust == null) return NotFound();

            return View(cust);
        }

        // ========== XỬ LÝ XÓA KHÁCH HÀNG ==========
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cust = _dbc.Users.Find(id);
            {
                _dbc.Users.Remove(cust);
                _dbc.SaveChanges();
            }

            return RedirectToAction("Index");
        }









    }
}
