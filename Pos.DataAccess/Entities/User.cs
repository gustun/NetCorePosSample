using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pos.Core.Interface;

namespace Pos.DataAccess.Entities
{
    public class User : IEntity, IStamp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public virtual User UpdatedUser { get; set; }
    }
}
