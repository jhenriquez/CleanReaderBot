using System;

namespace ReaderBot.Application.Common.Entities
{
    public class Author
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";

        public override bool Equals(object obj)
        {
            return obj is Author author &&
                   Id == author.Id &&
                   Name == author.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}