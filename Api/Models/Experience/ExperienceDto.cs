using System;
namespace Api.Models.Experience
{
    public class ExperienceDto
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Boolean Current { get; set; }
        public string Description { get; set; }
    }
}
