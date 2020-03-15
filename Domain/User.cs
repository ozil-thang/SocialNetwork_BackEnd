using System;
namespace Domain
{
    public class User
    {
        public User()
        {
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime Date { get; set; }

        public Profile Profile { get; set; }
    }
}
