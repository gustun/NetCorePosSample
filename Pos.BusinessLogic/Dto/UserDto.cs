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

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public UserDto CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public UserDto UpdatedUser { get; set; }
    }
}
