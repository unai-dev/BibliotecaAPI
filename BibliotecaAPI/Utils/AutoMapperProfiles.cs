using AutoMapper;
using BibliotecaAPI.DTOs.Authors;
using BibliotecaAPI.DTOs.AuthorsBooks;
using BibliotecaAPI.DTOs.Books;
using BibliotecaAPI.DTOs.Coments;
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

            CreateMap<AuthorBook, BooksDTO>().ForMember(dto => dto.Id, config => config.MapFrom(ent => ent.BookId))
                .ForMember(dto => dto.Title, config => config.MapFrom(ent => ent.Book!.Title));

            CreateMap<AuthorBook, AuthorsDTO>().ForMember(dto => dto.Id, config => config.MapFrom(ent => ent.AuthorId))
                .ForMember(dto => dto.FullName, config => config.MapFrom(ent => MappedNameAndSurname(ent.Author!)));


            CreateMap<Book, BooksDTO>();

            CreateMap<BookCreationDTO, Book>().ForMember(ent => ent.Authors, config => config.MapFrom(
                dto => dto.AuthorsIds.Select(id => new AuthorBook { AuthorId = id })));

            CreateMap<Book, BookWithAuthorDTO>();

            CreateMap<BookCreationDTO, AuthorBook>().ForMember(ent => ent.Book, 
                config => config.MapFrom(dto => new Book { Title = dto.Title }));

            CreateMap<ComentsCreationDTO, Coments>();
            CreateMap<Coments, ComentsDTO>()
                .ForMember(dto => dto.UserEmail, config => config.MapFrom(ent => ent.User!.Email));

        }

        private string MappedNameAndSurname(Author author) => $"{author.Names} {author.Surnames}";
    }
}
