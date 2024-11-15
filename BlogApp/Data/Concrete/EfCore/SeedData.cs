using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();
            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Entity.Tag { Text = "web programlama", Url = "web-programlama", Color = TagColors.success },
                        new Entity.Tag { Text = "backend", Url = "backend", Color = TagColors.warning },
                        new Entity.Tag { Text = "frontend", Url = "frontend", Color = TagColors.secondary },
                        new Entity.Tag { Text = "fullstack", Url = "fullstack", Color = TagColors.warning },
                        new Entity.Tag { Text = "php", Url = "php", Color = TagColors.success },
                        new Entity.Tag { Text = "python", Url = "python", Color = TagColors.info }

                        );
                    context.SaveChanges();
                }
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new Entity.User { UserName = "yucufer",
                            imageUrl = "kullanici1.jpg",
                        Password = "yucufer",
                        Email = "yucufer@gmail.com",
                        Name = "Yusuf"
                        },
                        new Entity.User { UserName = "sadıkturan",
                            imageUrl = "kullanici2.jpg",
                            Password = "sadıkturan",
                            Email = "sadık@gmail.com",
                            Name = "Sadık"
                        },
                        new Entity.User { UserName = "ahmetyılmaz",
                            imageUrl = "noUser2.jpg",
                            Password = "ahmetyılmaz",
                            Email = "ahmey@gmail.com",
                            Name = "Ahmet"
                        },
                        new Entity.User { UserName = "huseyinkartal",
                            imageUrl = "noUser2.jpg",
                            Password = "huseyinkartal",
                            Email = "huseyin@gmail.com",
                            Name = "Hüseyin"
                        }

                        );
                    context.SaveChanges();
                }
                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Entity.Post
                        {
                            Title = "Asp .Net",
                            Content = "Asp .net core dersleri",
                            Description = "Asp .Net Core Dersleri",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            UserId = 1,
                            ImageUrl = "5.jpg",
                            Url = "aspnet-core",
                            Comments = new List<Comment> {
                             new Comment {Text = "İyi bir kurs",PublishedOn = DateTime.Now.AddDays(-20),UserId = 1 } ,
                            new Comment {Text = "Çok faydalandığım bir kurs",PublishedOn = DateTime.Now.AddDays(-15),UserId = 2 },
                             new Comment {Text = "Hayatımı değiştiren kurslardan",PublishedOn = DateTime.Now.AddDays(-10),UserId = 3 }
                            }

                        },
                        new Entity.Post
                        {
                            Title = "Php Core",
                            Content = "Php core dersleri",
                            Description = "Php core dersleri",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1,
                            ImageUrl = "6.jpg",
                            Url = "php-core"
                        },
                        new Entity.Post
                        {
                            Title = "Django",
                            Content = "Django dersleri",
                            Description = "Django dersleri",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 1,
                            ImageUrl = "3.jpg",
                            Url = "django"
                        },
                        new Entity.Post
                        {
                            Title = "React Dersleri",
                            Content = "React dersleri",
                            Description = "React dersleri",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-15),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1,
                            ImageUrl = "4.jpg",
                            Url = "react"
                        },
                        new Entity.Post
                        {
                            Title = "Angular Dersleri",
                            Content = "Angular dersleri",
                            Description = "Angular dersleri",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-55),
                            Tags = context.Tags.Take(1).ToList(),
                            UserId = 1,
                            ImageUrl = "5.jpg",
                            Url = "angular"
                        },
                        new Entity.Post
                        {
                            Title = "Flask Dersleri",
                            Content = "Flask dersleri",
                            Description = "Flask dersleri",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-22),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 1,
                            ImageUrl = "2.jpg",
                            Url = "flask"
                        }

                        );
                    context.SaveChanges();
                }

            }
        }
    }
}
