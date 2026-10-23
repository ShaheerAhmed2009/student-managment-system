using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdManagment.Data;

namespace StdManagment.Controllers
{
    // Only users with the "Admin" role can open anything in this controller
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Admin  ->  redirects straight to the Students page
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Students));
        }

        // GET: /Admin/Students?search=ali
        public async Task<IActionResult> Students(string? search)
        {
            var query = _db.Students
                .AsNoTracking()
                .Where(s => s.Role == "Student");

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(s =>
                    s.FullName.Contains(search) ||
                    s.Email.Contains(search) ||
                    s.Phone.Contains(search) ||
                    s.StudentId.Contains(search));
            }

            ViewBag.Search = search;
            ViewBag.Total = await _db.Students.CountAsync(s => s.Role == "Student");
            ViewBag.ActiveCount = await _db.Students.CountAsync(s => s.Role == "Student" && s.IsActive);

            var students = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
            return View(students);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _db.Students.FindAsync(id);

            // Admin accounts can never be deleted from the panel
            if (student != null && student.Role == "Student")
            {
                _db.Students.Remove(student);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Student record deleted.";
            }

            return RedirectToAction(nameof(Students));
        }
    }
}