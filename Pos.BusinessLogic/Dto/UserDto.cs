using Pos.BusinessLogic.Dto.Base;
using System;

namespace Pos.BusinessLogic.Dto
{
    public class UserDto : Result
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
