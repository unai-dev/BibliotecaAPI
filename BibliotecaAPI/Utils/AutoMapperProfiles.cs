using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entitys;

namespace BibliotecaAPI.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Author, AuthorsDTO>()
                .ForMember(dto => dto.FullName, config => config.MapFrom(
                    author => MappedNameAndSurname(author)));
            CreateMap<Author, AuthorsWithBooksDTO>()
                .ForMember(dto => dto.FullName, config => config.MapFrom(
                    author => MappedNameAndSurname(author)));

            CreateMap<AuthorCreationDTO, Author>();


            CreateMap<Book, BooksDTO>();
            CreateMap<BookCreationDTO, Book>();
            CreateMap<Book, BookWithAuthorDTO>().ForMember(
                dto => dto.AuthorName, config => config.MapFrom(ent => MappedNameAndSurname(ent.Author)));
        }

        private string MappedNameAndSurname(Author author) => $"{author.Names} {author.Surnames}";
    }
}
