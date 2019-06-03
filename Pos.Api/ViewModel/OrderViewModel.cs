using System;
using System.Collections.Generic;

namespace Pos.Api.ViewModel
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public ICollection<OrderProductViewModel> OrderProducts { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedUserName { get; set; }
    }

    public class NewOrderViewModel
    {
        public string CustomerName { get; set; }
        public string CampaignCode { get; set; }
        public List<NewOrderProductViewModel> ProductList { get; set; }
    }
}
