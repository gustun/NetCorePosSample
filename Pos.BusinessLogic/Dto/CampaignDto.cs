using System;
using Pos.BusinessLogic.Dto.Base;
using Pos.Core.Enum;

namespace Pos.BusinessLogic.Dto
{
    public class CampaignDto : Result
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public EDiscountType DiscounType { get; set; }
        public decimal DiscountValue { get; set; }
        public int? MaxUsageCount { get; set; }
        public int? UsageCount { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public UserDto CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public UserDto UpdatedUser { get; set; }
    }
}
