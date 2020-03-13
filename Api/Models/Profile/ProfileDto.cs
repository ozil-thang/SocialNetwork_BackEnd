using System;
using System.Collections.Generic;
using Api.Models.Education;
using Api.Models.Experience;
using Domain;

namespace Api.Models.Profile
{
    public class ProfileDto
    {
        public ProfileDto()
        {


    }
        public string UserId { get; set; }

        public string Company { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public ICollection<string> Skills { get; set; }
        public string Bio { get; set; }
        public string GithubUsername { get; set; }
        public ICollection<ExperienceDto> Experiences { get; set; }
        public ICollection<EducationDto> Educations { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }

        public string Avatar { get; set; }
    }
}
