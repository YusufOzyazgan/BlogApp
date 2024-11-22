using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        //private readonly ILogger _logger;

        private readonly BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //_logger.Log(LogLevel.Error, "Veri Tabanına Kayıt Eklenirken Hata Oluştu "+ex);
                Console.WriteLine("Veri Tabanına Kayıt Eklenirken Hata Oluştu " + ex);
            }
           

        }
        public async Task EditPost(Post post, int[] tagIds)
        {
            var entity = await _context.Posts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.PostId == post.PostId);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;  
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.ImageUrl = post.ImageUrl;
                entity.IsActive = post.IsActive;
                entity.Tags = _context.Tags.Where(tag => tagIds.Contains(tag.TagId)).ToList();
                await _context.SaveChangesAsync();
            }
            else
            {
             
            }
        }

       
    }
}
