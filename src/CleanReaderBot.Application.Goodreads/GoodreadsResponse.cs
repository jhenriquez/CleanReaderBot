using System.Xml.Serialization;
using CleanReaderBot.Application.Common.Entities;

namespace CleanReaderBot.Application.Goodreads
{
  [XmlRoot("GoodreadsResponse")]
  public class GoodreadsResponse
  {
    [XmlElement("search")]
    public GoodreadsResponseSearch Result { get; set; }
  }

  public class GoodreadsResponseSearch
  {
    [XmlArray("results"), XmlArrayItem("work")]
    public GoodreadsResponseWork[] Works { get; set; }
  }

  public class GoodreadsResponseWork
  {
    [XmlElement("best_book")]
    public GoodreadsResponseBook BestBook { get; set; }
  }

  public class GoodreadsResponseBook {
      [XmlElement("id")]
      public int Id { get; set; }
      [XmlElement("title")]
      public string Title { get; set; }
  }
}