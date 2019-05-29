using System;
using System.Collections.Generic;
using Pos.Core.Interface;

namespace Pos.DataAccess.Entities
{
    public class Product : IEntity, IStamp
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public virtual User UpdatedUser { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
