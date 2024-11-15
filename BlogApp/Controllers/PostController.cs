﻿using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostController : Controller
    {



        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public PostController(IPostRepository postRepository, ICommentRepository commentRepository)
        {

            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts;
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
    }
}