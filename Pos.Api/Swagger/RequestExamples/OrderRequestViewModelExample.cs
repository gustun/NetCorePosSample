using System.Collections.Generic;
using Pos.Api.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace Pos.Api.Swagger.RequestExamples
{
    public class OrderRequestViewModelExample : IExamplesProvider<NewOrderViewModel>
    {
        public NewOrderViewModel GetExamples()
        {
            return new NewOrderViewModel
            {
                CustomerName = "Gokcan Ustun",
                CampaignCode = "CMP01",
                ProductList = new List<NewOrderProductViewModel>
                {
                    new NewOrderProductViewModel
                    {
                        ProductCode = "M001",
                        Quantity = 2
                    },
                    new NewOrderProductViewModel
                    {
                        ProductCode = "M002",
                        Quantity = 1
                    }
                }
            };
        }
    }
}