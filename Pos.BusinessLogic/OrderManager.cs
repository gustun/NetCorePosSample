using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos.BusinessLogic.DesignPatterns.Factory;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Dto.Base;
using Pos.BusinessLogic.Interface;
using Pos.DataAccess;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic
{
    public class OrderManager : IOrderManager
    {
        private readonly PosDbContext _context;
        private readonly IMapper _mapper;

        public OrderManager(PosDbContext posDbContext, IMapper mapper)
        {
            _context = posDbContext;
            _mapper = mapper;
        }

        public OrderDto Get(Guid id)
        {
            var toReturn = new OrderDto();
            var order = _context.Orders
                .AsNoTracking()
                .SingleOrDefault(x => x.Id == id);

            GetOrderRelationalData(order);
            return _mapper.Map(order, toReturn);
        }

        private void GetOrderRelationalData(Order order)
        {
            if (order == null)
                return;

            order.OrderProducts = _context.OrderProducts.AsNoTracking().Where(x => x.OrderId == order.Id).ToList();
            order.CreatedUser = _context.Users.AsNoTracking().SingleOrDefault(x => x.Id == order.CreatedUserId);
            foreach (var op in order.OrderProducts)
                op.Product = _context.Products.SingleOrDefault(x => x.Id == op.ProductId);
        }

        public OrderDto Add(NewOrderDto dto)
        {
            var toReturn = new OrderDto();
            var core = new Order {CustomerName = dto.CustomerName};
            foreach (var newOrderProductDto in dto.ProductList)
            {
                var product = _context.Products.SingleOrDefault(x => x.Code == newOrderProductDto.ProductCode);
                if (product == null) return toReturn.AddError("Product not found!");
                var orderProductCore = new OrderProduct
                {
                    ProductId = product.Id,
                    ProductUnitPrice = product.Price,
                    Quantity = newOrderProductDto.Quantity,
                    TotalAmount = newOrderProductDto.Quantity * product.Price
                };

                core.OrderProducts.Add(orderProductCore);
                core.TotalAmount += orderProductCore.TotalAmount;
            }

            if (!string.IsNullOrEmpty(dto.CampaignCode))
            {
                var campaign = _context.Campaigns.SingleOrDefault(x => x.Code == dto.CampaignCode);
                if (campaign == null) return toReturn.AddError("Invalid Campaign Code!");

                if (campaign.MaxUsageCount.HasValue && campaign.UsageCount >= campaign.MaxUsageCount)
                    return toReturn.AddError("Campaign max usage count is exceeded!");

                var oldPrice = core.TotalAmount;
                var newPrice = CampaginCalculatorFactory.Create(campaign.DiscounType).ApplyCampaign(oldPrice, campaign.DiscountValue);
                core.TotalAmount = newPrice;
                core.DiscountTotal = oldPrice - newPrice;
                campaign.UsageCount++;
            }

            _context.Orders.Add(core);
            _context.SaveChanges();
            GetOrderRelationalData(core);
            return _mapper.Map(core, toReturn);
        }

        public List<OrderProductDto> GetOrderItems(Guid orderId)
        {
            var orderItemList = _context.OrderProducts.AsNoTracking().Where(x => x.OrderId == orderId).ToList();
            foreach (var op in orderItemList)
                op.Product = _context.Products.SingleOrDefault(x => x.Id == op.ProductId);

            return _mapper.Map<List<OrderProductDto>>(orderItemList.ToList());
        }
    }
}
