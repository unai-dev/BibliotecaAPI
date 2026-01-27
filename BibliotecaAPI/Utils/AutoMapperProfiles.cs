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
                    author => $"{author.Names} {author.Surnames}"));
        }
    }
}
