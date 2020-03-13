using System;
namespace Domain
{
    public class Education
    {
        public Education()
        {
        }
        public string Id { get; set; }
        public string School { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Boolean Current { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
    }
}
