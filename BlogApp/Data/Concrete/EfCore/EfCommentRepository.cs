using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCommentRepository : ICommentRepository
    {
        //private readonly ILogger _logger;

        private readonly BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            
            _context = context;
        }
        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            try
            {
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Veri Tabanına Kayıt Eklenirken Hata Oluştu " + ex);
            }
           

        }
    }
}
