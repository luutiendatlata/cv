using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo5.Data;
using Demo5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Demo5.ViewModel;


namespace Demo5.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly UserManager<ApplicationUser> _userManager;
        public PostsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.Posts
                                       .Include(p => p.user)
                                       .Include(p => p.category)
                                       .OrderByDescending(p => p.postTime);
            
            
            var posts = applicationDbContext.Where(s => s.userId == userId);
            int pageSize = 3;
            return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));
           
        }

        // GET: Posts/Details/5
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
                .Where(c => c.postId ==id)
                .OrderByDescending(c=> c.Id);
            
            if (post == null)
            {
                return NotFound();
            }
            ViewBag.Comments = await comment.ToListAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.User = userId;
            
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["categories"] = new SelectList(_context.Categories, "Id", "name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("title,content,imageName,userId,categoryId,Id")] Post post)
        {
            if (ModelState.IsValid)
            {
                //get user id
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                post.userId = userId;
                //add post time
                post.postTime = DateTime.Now;
                //Insert image
                var files = HttpContext.Request.Form.Files;
                if (files != null) //check if there is uploaded img
                {
                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            var file = Image;
                            var uploads = Path.Combine(_hostEnvironment.WebRootPath, "img");
                            if (file.Length > 0)
                            {
                                var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            
                                {
                                    await file.CopyToAsync(fileStream);
                                    post.imageName = fileName;
                                }
                            }
                        }
                    }
                }
                else
                {
                    post.imageName = "";
                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", post.userId);
            ViewData["categories"] = new SelectList(_context.Categories, "Id", "name", post.categoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", post.userId);
            ViewData["categories"] = new SelectList(_context.Categories, "Id", "name", post.categoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("title,content,postTime,imageName,userId,categoryId,Id")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //delete old image
                    
                    if (post.imageName == null)
                    {
                        return NotFound();
                    }
                    var imgPath = Path.Combine(_hostEnvironment.WebRootPath, "img");
                    if (post.imageName != null)
                    {
                        System.IO.File.Delete(Path.Combine(imgPath, post.imageName));
                    }

                    //add new image
                    var files = HttpContext.Request.Form.Files;
                    if (files != null) //check if there is uploaded img
                    {
                        foreach (var Image in files)
                        {
                            if (Image != null && Image.Length > 0)
                            {
                                var file = Image;
                                var uploads = Path.Combine(_hostEnvironment.WebRootPath, "img");
                                if (file.Length > 0)
                                {
                                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))

                                    {
                                        await file.CopyToAsync(fileStream);
                                        post.imageName = fileName;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        post.imageName = "";
                    }
                    //update updated time
                    post.updTime =DateTime.Now;

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", post.userId);
            ViewData["categories"] = new SelectList(_context.Categories, "Id", "name", post.category.name);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.user)
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            //delete file
            if (post.imageName != null )
            {
                var imgPath = Path.Combine(_hostEnvironment.WebRootPath, "img");
                System.IO.File.Delete(Path.Combine(imgPath, post.imageName));
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id,string content)
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
            //ViewData["postId"] = new SelectList(_context.Posts, "Id", "Id", comment.postId);
            //ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", comment.userId);
            //return View(comment);
            var post = await _context.Posts
                .Include(p => p.user)
                .Include(p => p.category)
                .FirstOrDefaultAsync(m => m.Id == id);

            var cm = _context.Comments
                .Include(c => c.post)
                .Include(c => c.user)
                .Where(c => c.postId == id);

            if (post == null)
            {
                return NotFound();
            }
            var postView = new PostDetailsViewModel();
            postView.posts = post;
            postView.comments = await cm.ToListAsync();
            return View(postView);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

    }
}
