using Api.Dtos.Dependent;
using Api.Models;
using AutoMapper;

namespace Api.Mappers
{
    public class DependentProfile : Profile
    {
        public DependentProfile()
        {
            CreateMap<Dependent, GetDependentDto>();
        }
    }
}
