using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfTagRepository : ITagRepository
    {
        

        private readonly BlogContext _context;
        public EfTagRepository(BlogContext context)
        {
            
            _context = context;
        }
        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag tag)
        {
            try
            {
                _context.Tags.Add(tag);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Veri Tabanına Kayıt Eklenirken Hata Oluştu " + ex);
            }
           

        }
    }
}
