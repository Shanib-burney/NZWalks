using AutoMapper;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Source , Destination
            CreateMap<Region, RegionDTO>().ReverseMap();

            //CreateMap<RegionRequestDTO, Region>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<RegionRequestDTO, Region>().ReverseMap();

            CreateMap<Walk, WalkDTO>().ReverseMap();


            CreateMap<WalkRequestDTO, Walk>().ReverseMap();

            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();




        }
    }
}
