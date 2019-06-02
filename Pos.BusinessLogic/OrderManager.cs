using System;
using AutoMapper;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Interface;
using Pos.DataAccess;

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
            throw new NotImplementedException();
        }

        public OrderDto Add(OrderDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
