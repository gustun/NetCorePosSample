using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Dto.Base;
using Pos.BusinessLogic.Interface;
using Pos.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos.BusinessLogic.DesignPatterns.Builder;
using Pos.BusinessLogic.Dto.Common;
using Pos.DataAccess;

namespace Pos.BusinessLogic
{
    public class ProductManager : IProductManager
    {
        private readonly PosDbContext _context;
        private readonly IMapper _mapper;

        public ProductManager(PosDbContext posDbContext, IMapper mapper)
        {
            _context = posDbContext;
            _mapper = mapper;
        }

        public ProductDto Add(ProductDto dto)
        {
            if (_context.Products.Any(x => x.Code == dto.Code))
                return dto.AddError("Product already exists!");

            var entity = _mapper.Map<Product>(dto);
            _context.Products.Add(entity);
            _context.SaveChanges();
            dto = _mapper.Map<ProductDto>(entity);
            return dto.AddSuccess();
        }

        public Result Update(ProductDto dto)
        {
            var result = new Result();
            var entity = Get(dto.Id);
            if (entity == null)
                return result.AddError("Product not found!");

            if (_context.Products.Any(x => x.Code == dto.Code && x.Id != dto.Id))
                return dto.AddError($"The code '{dto.Code}' is already being used!");

            var newEntity = _mapper.Map<Product>(dto);
            _context.Products.Update(newEntity);
            _context.SaveChanges();
            return result.AddSuccess();
        }

        public Result Delete(Guid entityId)
        {
            var result = new Result();
            var entity = _context.Products.SingleOrDefault(x => x.Id == entityId);
            if (entity == null)
                return result.AddError("Product not found!");

            _context.Products.Remove(entity);
            _context.SaveChanges();
            return result.AddSuccess();
        }

        public ProductDto Get(Guid id)
        {
            return _mapper.Map<ProductDto>(_context.Products.AsNoTracking().SingleOrDefault(x => x.Id == id));
        }

        public ProductDto Get(string code)
        {
            return _mapper.Map<ProductDto>(_context.Products.AsNoTracking().SingleOrDefault(x => x.Code == code));
        }

        public bool IsExists(Guid entityId)
        {
            return _context.Products.Any(o => o.Id == entityId);
        }

        public PagedResult<ProductDto> GetWithPaging(int page, int pageSize, Expression<Func<Product, bool>> predicate = null)
        {
            if (predicate == null) predicate = entity => true;
            var query = _context.Products.AsNoTracking().Where(predicate);

            var result = new PagedResult<ProductDto> {CurrentPage = page, PageSize = pageSize, RowCount = query.Count()};
            result.PageCount = (int)Math.Ceiling((double)result.RowCount / pageSize);
 
            var skip = (page - 1) * pageSize;     
            var list = query.Skip(skip).Take(pageSize).ToList();
            foreach (var entity in list)
                result.Results.Add(_mapper.Map<ProductDto>(entity));

            return result;
        }

        //todo: add to interface and merge with getwithpaging
        public List<ProductDto> GetWithFilter(ProductFilterDto filterDto, int pageIndex, int pageSize)
        {
            var query = _context.Products.AsNoTracking();
            var filterBuilder = ProductFilterBuilder.Create(query)
                .SetSearchTerm(filterDto.SearchTerm)
                .SetPrice(filterDto.Price)
                .SetPaging(pageIndex, pageSize)
                .Build();

            var entities = filterBuilder.ToList();
            return _mapper.Map<List<ProductDto>>(entities);
        }

        public ProductFilterOptionsDto GetProductFilters()
        {
            return new ProductFilterOptionsDto
            {
                Price = new RangeDto<decimal>
                {
                    Min = _context.Products.Min(x => x.Price), 
                    Max = _context.Products.Max(x => x.Price)
                }
            };
        }
    }
}
