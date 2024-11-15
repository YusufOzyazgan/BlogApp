using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    }
}
