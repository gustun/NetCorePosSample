using System;
using System.Collections.Generic;
using Pos.BusinessLogic.Dto;

namespace Pos.BusinessLogic.Interface
{
    public interface IOrderManager
    {
        OrderDto Get(Guid id);
        OrderDto Add(NewOrderDto dto);
        List<OrderProductDto> GetOrderItems(Guid orderId);
    }
}
