using System;
using System.Linq;
using Api.Models.Education;
using Api.Models.Experience;
using Api.Models.Post;
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

            CreateMap<Domain.Post, PostItemDto>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserProfile.Avatar.Url))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserProfile.DisplayName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.Url))
                .ForMember(dest => dest.Video, opt => opt.MapFrom(src => src.Video.Url))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes.Count()))
                .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count()));

            CreateMap<Domain.Comment, CommentDto>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserProfile.Avatar.Url))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserProfile.DisplayName));

            CreateMap<Domain.Post, PostDetailDto>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserProfile.Avatar.Url))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserProfile.DisplayName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.Url))
                .ForMember(dest => dest.Video, opt => opt.MapFrom(src => src.Video.Url))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes.Count()));


        }
    }
}
