using System;
namespace Domain
{
    public class Experience
    {
        public Experience()
        {
            
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Boolean Current { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
    }
}
