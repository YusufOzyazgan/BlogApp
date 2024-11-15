using BlogApp.Entity;
namespace BlogApp.Data.Abstract
{
    public interface IPostRepository    
    {
        // context üzreinden postlar alındığı zaman extra filtrlemeye devam edilebilecek demek yani eğer Ienumareable deseydik tüm postları alıp onları filtreleyecekti anlamına geliyor fakat şuanda postları filtreli şeikilde alacağız
        IQueryable<Post> Posts { get; }

        void CreatePost(Post post);
    }
}
