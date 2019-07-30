using Windows_UWP.Enums;

namespace Windows_UWP.Entities
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public Business Business { get; set; }
    }
}