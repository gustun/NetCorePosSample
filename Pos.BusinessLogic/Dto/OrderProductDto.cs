using System;
using Pos.BusinessLogic.Dto.Base;

namespace Pos.BusinessLogic.Dto
{
    public class OrderProductDto : NewOrderProductDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class NewOrderProductDto : Result
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
