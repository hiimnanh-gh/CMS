using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMS.Models;

namespace CMS.Controllers.Admin
{

    public class StaffManagementController : Controller
    {
        private readonly CinemaDbContext _dbc;

        public StaffManagementController(CinemaDbContext db)
        {
            _dbc = db;
        }

        // ========== HIỂN THỊ DANH SÁCH NHÂN VIÊN ==========
        public IActionResult Index(string search, int page = 1)
        {
            int pageSize = 5; // Số nhân viên mỗi trang
            var query = _dbc.Users
                .Where(u => u.RoleId == 2) // Lọc chỉ lấy nhân viên
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
                ViewData["SearchQuery"] = search;
            }

            var staffList = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewData["CurrentPage"] = page;

            return View(staffList);
        }




        // Hiển thị form thêm nhân viên
        [HttpGet("CreateStaff")]
        public IActionResult CreateStaff()
        {
            return View();
        }

        // Xử lý thêm nhân viên
        [HttpPost("CreateStaff")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStaff(StaffCreateModel model)
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
                RoleId = 2 // ✅ Mặc định Staff
            };

            _dbc.Users.Add(newUser);
            _dbc.SaveChanges();

            return RedirectToAction("Index");
        }
         

        // ========== HIỂN THỊ FORM CHỈNH SỬA ==========
        [HttpGet("EditStaff/{id}")]
        public IActionResult EditStaff(int id)
        {
            var staff = _dbc.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == id);

            if (staff == null)
            {
                return NotFound();
            }

            ViewBag.Roles = _dbc.Roles.ToList(); // ✅ Lấy danh sách Role để hiển thị

            return View(staff);
        }

        [HttpPost("EditStaff/{id}")]
        public IActionResult EditStaff(int id, User staffData)
        {
            var existingStaff = _dbc.Users.FirstOrDefault(u => u.UserId == id);

            if (existingStaff == null)
            {
                return NotFound();
            }

            // ✅ Cập nhật thông tin
            existingStaff.UserName = staffData.UserName;
            existingStaff.Email = staffData.Email;
            existingStaff.PhoneNum = staffData.PhoneNum;

            // ✅ Nếu RoleId được chọn khác với RoleId hiện tại => cập nhật
            if (staffData.RoleId != existingStaff.RoleId)
            {
                existingStaff.RoleId = staffData.RoleId;
            }

            _dbc.Users.Update(existingStaff);
            _dbc.SaveChanges();

            return RedirectToAction("Index", "StaffManagement");
        }
















        // ========== HIỂN THỊ FORM XÁC NHẬN XÓA NHÂN VIÊN ==========
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var staff = _dbc.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserId == id && u.Role.RoleName == "Staff");

            if (staff == null) return NotFound();

            return View(staff);
        }



        // ========== XỬ LÝ XÓA NHÂN VIÊN ==========
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var staff = _dbc.Users.Find(id);
            {
                _dbc.Users.Remove(staff);
                _dbc.SaveChanges();
            }

            return RedirectToAction("Index");
        }









    }
}
