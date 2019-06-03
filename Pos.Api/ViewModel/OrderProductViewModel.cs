using System;

namespace Pos.Api.ViewModel
{
    public class OrderProductViewModel : NewOrderProductViewModel
    {
        public decimal ProductUnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class NewOrderProductViewModel
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
