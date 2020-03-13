using System;
using System.Linq;
using Api.Models.Education;
using Api.Models.Experience;
using Api.Models.Profile;
using Api.Models.User;

namespace Api.Models
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Profile, ProfileDto>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => s.Name)))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url));

            CreateMap<Domain.Profile, ProfileItemDto>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => s.Name)))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url));


            CreateMap<CreateProfileDto, Domain.Profile>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Split(',', StringSplitOptions.None)
                                                                                .AsEnumerable()
                                                                                .Select(s => new Domain.Skill { Name = s })))
                .ForMember(dest => dest.Avatar, opt => opt.Ignore());


            CreateMap<Domain.Experience, ExperienceDto>();
            CreateMap<Domain.Education, EducationDto>();

            CreateMap<Domain.User, UserDto>();

        }
    }
}
