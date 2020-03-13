using System;
namespace Domain
{
    public class Skill
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }
        public Profile UserProfile { get; set; }
    }
}
