using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1131754.Models;
using _1131754.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace _1131754.Controllers
{
    public class UsersController : Controller
    {
        private readonly CmsContext _context;

        public UsersController(CmsContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TableMovies1131754.ToListAsync());
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var user = _context.TableUsers1131754
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null &&
                !string.IsNullOrEmpty(user.Username) &&
                !string.IsNullOrEmpty(user.Role))
            {
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return RedirectToAction("Store", "Movies");
            }

            ViewBag.Error = "帳號或密碼錯誤";

            return View();
        }

        public IActionResult Order()
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "請先登入";

                return RedirectToAction("Index");
            }

            int userId = int.Parse(userIdStr);

            var orders = _context.TableOrders1131754
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Movie)
                .Where(o => o.UserID == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}