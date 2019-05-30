using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pos.Core.Interface;

namespace Pos.DataAccess.Entities
{
    public class Order : IEntity, IStamp
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountTotal { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public virtual User UpdatedUser { get; set; }
    }
}
