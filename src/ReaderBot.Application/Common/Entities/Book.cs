namespace ReaderBot.Application.Common.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Author[] Authors { get; set; }
        public double AverageRating { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
    }
}