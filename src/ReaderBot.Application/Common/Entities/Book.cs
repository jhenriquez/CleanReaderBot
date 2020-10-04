namespace ReaderBot.Application.Common.Entities
{
    public class Book
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public Author[] Authors { get; set; } = new Author[] { };
        public double AverageRating { get; set; } = 0;
        public string ImageUrl { get; set; } = "";
        public string SmallImageUrl { get; set; } = "";
        public string Description { get; set; } = "";
    }
}