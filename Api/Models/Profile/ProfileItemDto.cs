using System;
using System.Collections.Generic;

namespace Api.Models.Profile
{
    public class ProfileItemDto
    {
        public ProfileItemDto()
        {
        }
        public string UserId { get; set; }

        public string Company { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public ICollection<string> Skills { get; set; }

        public string Avatar { get; set; }
    }
}
