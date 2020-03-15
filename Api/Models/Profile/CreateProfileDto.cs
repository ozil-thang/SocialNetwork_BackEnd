using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Api.Models.Profile
{
    public class CreateProfileDto
    {
        public string DisplayName { get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Skills { get; set; }
        public string Bio { get; set; }
        public string GithubUsername { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
