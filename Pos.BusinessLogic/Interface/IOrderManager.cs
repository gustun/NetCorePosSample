using System;
using Pos.BusinessLogic.Dto;

namespace Pos.BusinessLogic.Interface
{
    public interface IOrderManager
    {
        OrderDto Get(Guid id);
        OrderDto Add(OrderDto dto);
    }
}
