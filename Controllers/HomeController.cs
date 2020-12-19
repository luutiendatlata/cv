using Demo5.Data;
using Demo5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Demo5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            
            
            var posts = _context.Posts
                        .Include(p => p.user)
                        .Include(p => p.category)
                        .OrderByDescending(p => p.postTime);
            
            int pageSize = 3;
            //ViewData["posts"] = posts.AsNoTracking();
            
            return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.user)
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            var comment = _context.Comments
                .Include(c => c.post)
                .Include(c => c.user)
                .Where(c => c.postId == id)
                .OrderByDescending(c => c.Id);

            if (post == null)
            {
                return NotFound();
            }
            ViewBag.Comments = await comment.ToListAsync();
            //get user id for comment checking
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = userId;
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(int id, string content)
        {

            if (ModelState.IsValid)
            {
                Comment comment = new Comment();
                comment.postId = id;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                comment.userId = userId;
                comment.content = content;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = id });
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

    }
}
