using System;
namespace Api.Models.Education
{
    public class EducationDto
    {
        public string Id { get; set; }
        public string School { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Boolean Current { get; set; }
        public string Description { get; set; }
    }
}
