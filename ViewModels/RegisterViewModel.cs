using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; } = UserType.Customer;
        public BusinessViewModel Business { get; set; } = new BusinessViewModel();
    }
}
