using AutoMapper;
using CleanReaderBot.Application.Common.Entities;

namespace CleanReaderBot.Application.Goodreads
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<GoodreadsResponseBook, Book>();
        }
    }
}