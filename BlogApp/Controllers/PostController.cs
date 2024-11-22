using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {



        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;
        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
        {

            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;   
        }
        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts.Where(x => x.IsActive == true);
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
                   
            }


            return View(new PostsViewModel { Posts = await posts.ToListAsync() });

        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository
                .Posts
                .Include(x => x.User)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var image = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userId ?? ""),

            };
            _commentRepository.CreateComment(entity);
            return Json(new
            {
                userName,
                Text,
                entity.PublishedOn,
                image,
            });


        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreatePostViewModel model)
        {

            if (ModelState.IsValid)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _postRepository.CreatePost(
                    new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        Description = model.Description,
                        Url = model.Url,
                        UserId = int.Parse(userID ?? ""),
                        PublishedOn = DateTime.Now,
                        ImageUrl = "5.jpg",
                        IsActive = false


                    });
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]

        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var posts = _postRepository.Posts;
            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(i => i.UserId == userId);
            }

            return View(await posts.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = await _postRepository.Posts.Include(x=> x.Tags).FirstOrDefaultAsync(i => i.PostId == id);

            if (post == null) return NotFound();
            var model = new EditPostViewModel()
            {
                PostId = (int)id,
                ImageUrl = post.ImageUrl,
                Title = post.Title,
                Description = post.Description,
                Url = post.Url,
                Content = post.Content,
                IsActive = post.IsActive,
                Tags = post.Tags,
            };
            ViewBag.Tags = await _tagRepository.Tags.ToListAsync(); 

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    var allowedExtention = new[] { ".jpg", ".jpeg", ".png" };
                    var extention = Path.GetExtension(model.Image.FileName);

                    if (!allowedExtention.Contains(extention))
                    {
                        ModelState.AddModelError("", "Lütfen geçerli formatta bir resim seçiniz!");
                        return View(model);
                    }
                    else
                    {
                        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extention}");
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                        model.ImageUrl = randomFileName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.Image.CopyToAsync(stream);
                        }
                    }
                   
                }
                var entity = new Post()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Url = model.Url,
                    ImageUrl = model.ImageUrl,
                    Content = model.Content,
                    PostId = model.PostId,
                    IsActive = model.IsActive
                };

                await _postRepository.EditPost(entity,tagIds);

                return RedirectToAction("index");

            }
            return View(model);

        }
    }
}
