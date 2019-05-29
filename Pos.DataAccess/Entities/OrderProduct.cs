using System;
using Pos.Core.Interface;

namespace Pos.DataAccess.Entities
{
    public class OrderProduct : IEntity, IStamp
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public virtual User UpdatedUser { get; set; }
    }
}
