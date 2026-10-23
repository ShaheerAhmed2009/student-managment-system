using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StdManagment.Data;
using StdManagment.Models;
using StdManagment.ViewModels;

namespace StdManagment.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<Student> _hasher;

    public AccountController(AppDbContext db, IPasswordHasher<Student> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    private static string NormalizePhone(string phone)
    {
        phone = phone.Trim();
        return phone.StartsWith("+92") ? "0" + phone.Substring(3) : phone;
    }

    // ---------- REGISTER ----------
    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var email = model.Email.Trim();
        var studentId = model.StudentId.Trim();

        if (await _db.Students.AnyAsync(s => s.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email already registered.");
            return View(model);
        }
        if (await _db.Students.AnyAsync(s => s.StudentId == studentId))
        {
            ModelState.AddModelError(nameof(model.StudentId), "Student ID already registered.");
            return View(model);
        }

        var student = new Student
        {
            FullName = model.FullName.Trim(),
            Email = email,
            Phone = NormalizePhone(model.Phone),
            StudentId = studentId,
            Role = "Student"      // hamesha Student
        };
        student.PasswordHash = _hasher.HashPassword(student, model.Password);

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Account created. Please sign in.";
        return RedirectToAction(nameof(Login));
    }

    // ---------- LOGIN ----------
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);

        var id = model.Identifier.Trim();
        var phone = NormalizePhone(id);

        var user = await _db.Students.FirstOrDefaultAsync(s =>
            s.Email == id || s.Phone == phone || s.StudentId == id);

        if (user == null ||
            _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password)
                == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View(model);
        }

        user.IsActive = true;
        user.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = model.RememberMe });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return user.Role == "Admin"
            ? RedirectToAction("Index", "Admin")
            : RedirectToAction("Index", "Home");
    }

    // ---------- LOGOUT ----------
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(idClaim, out var uid))
        {
            var user = await _db.Students.FindAsync(uid);
            if (user != null)
            {
                user.IsActive = false;
                await _db.SaveChangesAsync();
            }
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();
}