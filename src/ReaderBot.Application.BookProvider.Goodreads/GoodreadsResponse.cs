using System.Xml.Serialization;
using ReaderBot.Application.Common.Entities;

namespace ReaderBot.Application.BookProvider.Goodreads
{
  [XmlRoot("GoodreadsResponse")]
  public class GoodreadsResponse
  {
    [XmlElement("search")]
    public GoodreadsResponseSearch Search { get; set; }
    [XmlElement("book")]
    public GoodreadsResponseBook Book { get; set; }
  }

  public class GoodreadsResponseSearch
  {
    [XmlArray("results"), XmlArrayItem("work")]
    public GoodreadsResponseWork[] Works { get; set; }
  }

  public class GoodreadsResponseWork
  {
    [XmlElement("average_rating")]
    public float AverageRating { get; set; }
    [XmlElement("best_book")]
    public GoodreadsResponseBook BestBook { get; set; }
  }

  public class GoodreadsResponseBook {
      [XmlElement("id")]
      public int Id { get; set; }
      [XmlElement("title")]
      public string Title { get; set; }
      [XmlElement("author")]
      public GoodreadsResponseAuthor Author { get; set;}
      [XmlElement("image_url")]
      public string ImageUrl { get; set; }
      [XmlElement("small_image_url")]
      public string SmallImageUrl { get; set; }
      [XmlElement("average_rating")]
      public double AverageRating { get; set; }

      [XmlArray("authors"), XmlArrayItem("author")]
      public GoodreadsResponseAuthor[] Authors { get; set; }
  }

  public class GoodreadsResponseAuthor {
    [XmlElement("id")]
      public int Id { get; set; }
      [XmlElement("name")]
      public string Name { get; set; }
  }
}