using task2.Models;

namespace task2.Context;

public static class Initializer
{
    public static void InitDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public static void Seed(AppDbContext context)
    {
        var rate1 = new Rating { Score = 4.5m, };
        var rate2 = new Rating { Score = 2.5m, };
        var rate4 = new Rating { Score = 3.5m, };
        var rate3 = new Rating { Score = 1.5m, };
        var rate5 = new Rating { Score = 9.5m, };
        var rate6 = new Rating { Score = 10m, };

        
        //var review1 = new Review { Message = "Message1", Reviewer = "Reviewer1" };
        //var review2 = new Review { Message = "Message2", Reviewer = "Reviewer1" };
        //var review3 = new Review { Message = "Message3", Reviewer = "Reviewer2" };
        //var review4 = new Review { Message = "Message4", Reviewer = "Reviewer2" };
        //var review5 = new Review { Message = "Message1", Reviewer = "Reviewer3" };
        //var review6 = new Review { Message = "Message6", Reviewer = "Reviewer6" };
        //var review8 = new Review { Message = "Message8", Reviewer = "Reviewer8" };
        //var review7 = new Review { Message = "Message7", Reviewer = "Reviewer7" };
        //var review9 = new Review { Message = "Message9", Reviewer = "Reviewer9" };
        //var review10 = new Review { Message = "Message10", Reviewer = "Reviewer10" };
        //var review11 = new Review { Message = "Message11", Reviewer = "Reviewer11" };
        //var review12 = new Review { Message = "Message12", Reviewer = "Reviewer12" };
        //var review13 = new Review { Message = "Message13", Reviewer = "Reviewer13" };
        //var review14 = new Review { Message = "Message14", Reviewer = "Reviewer14" };
        //var review15 = new Review { Message = "Message15", Reviewer = "Reviewer15" };
        //var review16 = new Review { Message = "Message16", Reviewer = "Reviewer16" };

        var reviews = new List<Review>();
        for (int i = 0; i < 33; i++)
        {
            reviews.Add(new Review
            {
                Message = $"Message{i}",
                Reviewer = $"Reviewer{i}"
            });
        }


        var book3 = new Book { Title = "Book3", Author = "AbbaAuthor3", Content = "Content3", Cover = "Cover3", Genre = "Genre3" };
        var book1 = new Book { Title = "Book1", Author = "BBAuthor1", Content = "Content1", Cover = "Cover1", Genre = "Genre1" };
        var book4 = new Book { Title = "Book4", Author = "RdAuthor4", Content = "Content4", Cover = "Cover4", Genre = "Genre4" };
        var book5 = new Book { Title = "Book5", Author = "DaadAuthor5", Content = "Content5", Cover = "Cover5", Genre = "Genre5" };
        var book2 = new Book { Title = "Book2", Author = "CCAuthor2", Content = "Content2", Cover = "Cover2", Genre = "Genre2" };
        
        context.Ratings.AddRange(rate1,rate2,rate3,rate4,rate5,rate6);
        context.Reviews.AddRange(reviews);
        context.Books.AddRange(book1, book2, book3, book4, book5);

        context.Entry(book1).Collection(prop => prop.Reviews).Load();
        for (int i = 0; i < 11; i++)
        {
            book1.Reviews.Add(reviews[i]);
        }
        context.Entry(book1).Collection(prop => prop.Ratings).Load();
        book1.Ratings.Add(rate6);
        book1.Ratings.Add(rate1);

        context.Entry(book3).Collection(prop => prop.Reviews).Load();
        for (int i = 11; i < 22; i++)
        {
            book3.Reviews.Add(reviews[i]);
        }
        context.Entry(book3).Collection(prop => prop.Ratings).Load();
        book3.Ratings.Add(rate5);
        book3.Ratings.Add(rate2);

        context.Entry(book5).Collection(prop => prop.Reviews).Load();
        for (int i = 22; i < 33; i++)
        {
            book5.Reviews.Add(reviews[i]);
        }
        context.Entry(book5).Collection(prop => prop.Ratings).Load();

        context.SaveChanges();
    }
}
