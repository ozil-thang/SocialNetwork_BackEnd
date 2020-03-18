using System;
using System.Collections.Generic;

namespace Domain
{
    public class Profile
    {
        public Profile()
        {
            Skills = new List<Skill>();
            Experiences = new List<Experience>();
            Educations = new List<Education>();
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }
        public string UserId { get; set; }
        public User User { get; set; }

        public string DisplayName { get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public string Bio { get; set; }
        public string GithubUserName { get; set; }
        public ICollection<Experience> Experiences { get; set; }
        public ICollection<Education> Educations { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }

        public string AvatarId { get; set; }
        public Photo Avatar { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
