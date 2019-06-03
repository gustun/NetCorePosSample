using System;
using Pos.Core.Enum;
using Pos.Core.Interface;

namespace Pos.DataAccess.Entities
{
    public class Campaign : IEntity, IStamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public EDiscountType DiscounType { get; set; }
        public decimal DiscountValue { get; set; }
        public int? MaxUsageCount { get; set; }
        public int UsageCount { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public virtual User UpdatedUser { get; set; }
    }
}
