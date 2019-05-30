using System.ComponentModel.DataAnnotations;

namespace Pos.Api.ViewModel
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }

    public class UserCreateModel : UserModel
    {
        [Required]
        public string Password { get; set; }
    }
}
