using System;
using Pos.BusinessLogic.Dto.Base;

namespace Pos.BusinessLogic.Dto
{
    public class OrderProductDto : Result
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public OrderDto Order { get; set; }

        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }

        public int Quantity { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public UserDto CreatedUser { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedUserId { get; set; }
        public UserDto UpdatedUser { get; set; }
    }
}
