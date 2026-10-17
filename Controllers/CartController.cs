using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1131754.Models;
using _1131754.Data;

namespace _1131754.Controllers
{
    public class CartController : Controller
    {
        private readonly CmsContext _context;

        public CartController(CmsContext context)
        {
            _context = context;
        }

        // 顯示購物車頁面
        public IActionResult Shop()
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "請先登入才能查看購物車";

                return RedirectToAction("Index", "Users");
            }

            int userId = int.Parse(userIdStr);

            var cartItems = _context.TableCartItems1131754
                .Include(ci => ci.Movie)
                .Where(ci => ci.UserID == userId)
                .ToList();

            return View(cartItems);
        }

        // 加入購物車
        public IActionResult AddToCart(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "請先登入才能加入購物車";

                return RedirectToAction("Index", "Users");
            }

            int userId = int.Parse(userIdStr);

            var movie = _context.TableMovies1131754
                .FirstOrDefault(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var existingItem = _context.TableCartItems1131754
                .FirstOrDefault(ci =>
                    ci.MovieID == id &&
                    ci.UserID == userId);

            if (existingItem == null)
            {
                var newItem = new Cart
                {
                    MovieID = movie.MovieID,
                    Quantity = 1,
                    Discount = 0,
                    UserID = userId
                };

                _context.TableCartItems1131754.Add(newItem);
            }
            else
            {
                existingItem.Quantity += 1;
            }

            _context.SaveChanges();

            TempData["Success"] = "電影已加入購物車";

            return RedirectToAction("Store", "Movies");
        }

        // 移除購物車商品
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "請先登入";

                return RedirectToAction("Index", "Users");
            }

            int userId = int.Parse(userIdStr);

            var item = _context.TableCartItems1131754
                .FirstOrDefault(ci =>
                    ci.MovieID == id &&
                    ci.UserID == userId);

            if (item != null)
            {
                _context.TableCartItems1131754.Remove(item);

                _context.SaveChanges();
            }

            return RedirectToAction("Shop");
        }

        // 結帳
        [HttpPost]
        public IActionResult Checkout()
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["Error"] = "請先登入";

                return RedirectToAction("Index", "Users");
            }

            int userId = int.Parse(userIdStr);

            var cartItems = _context.TableCartItems1131754
                .Include(ci => ci.Movie)
                .Where(ci => ci.UserID == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "購物車是空的，無法結帳";

                return RedirectToAction("Shop");
            }

            // 建立訂單
            var order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.Now,

                OrderDetails = cartItems.Select(ci => new OrderDetail
                {
                    MovieID = ci.MovieID,
                    Quantity = ci.Quantity,
                    Price = ci.Movie.Price
                }).ToList()
            };

            _context.TableOrders1131754.Add(order);

            // 清空購物車
            _context.TableCartItems1131754.RemoveRange(cartItems);

            _context.SaveChanges();

            TempData["Success"] =
                $"結帳成功！訂單編號：{order.OrderID}";

            return RedirectToAction("Shop");
        }
    }
}