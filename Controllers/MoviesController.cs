using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1131754.Models;
using _1131754.Data;
using System.Linq;
using System.Threading.Tasks;

namespace _1131754.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CmsContext _context;

        public MoviesController(CmsContext context)
        {
            _context = context;
        }

        // Movie Store
        public IActionResult Store(string search = "", string Type = "", int page = 1)
        {
            const int PageSize = 8;

            // 初始查詢
            var query = _context.TableMovies1131754.AsQueryable();

            // 搜尋關鍵字
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m =>
                    m.Name.Contains(search) ||
                    m.Description.Contains(search));
            }

            // 分類篩選
            if (!string.IsNullOrEmpty(Type))
            {
                query = query.Where(m => m.Type == Type);
            }

            // 計算總數
            var totalItems = query.Count();

            // 分頁
            var movies = query
                .OrderBy(m => m.MovieID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            // 傳給 View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages =
                (int)Math.Ceiling((double)totalItems / PageSize);

            ViewBag.Search = search;
            ViewBag.Type = Type;

            // 所有電影分類
            ViewBag.Types = _context.TableMovies1131754
                .Select(m => m.Type)
                .Distinct()
                .ToList();

            // Session Role
            ViewBag.Role = HttpContext.Session.GetString("Role");

            return View(movies);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(movie);
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Store));
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie =
                await _context.TableMovies1131754.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.MovieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(movie);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Store));
            }

            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.TableMovies1131754
                .FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie =
                await _context.TableMovies1131754.FindAsync(id);

            if (movie == null)
            {
                TempData["Error"] = "找不到要刪除的電影。";

                return RedirectToAction(nameof(Store));
            }

            _context.TableMovies1131754.Remove(movie);

            await _context.SaveChangesAsync();

            TempData["Success"] = "電影已成功刪除。";

            return RedirectToAction(nameof(Store));
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.TableMovies1131754
                .FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}