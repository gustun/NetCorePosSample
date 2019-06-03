using System;
using System.Collections.Generic;
using Pos.BusinessLogic.Dto.Base;

namespace Pos.BusinessLogic.Dto
{
    public class OrderDto : Result
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CampaignCode { get; set; }
        public ICollection<OrderProductDto> OrderProducts { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public UserDto CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public UserDto UpdatedUser { get; set; }
    }

    public class NewOrderDto : Result
    {
        public string CustomerName { get; set; }
        public string CampaignCode { get; set; }
        public List<NewOrderProductDto> ProductList { get; set; }
    }
}
