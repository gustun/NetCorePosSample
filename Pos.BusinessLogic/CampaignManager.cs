using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Dto.Base;
using Pos.BusinessLogic.Interface;
using Pos.DataAccess;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic
{
    public class CampaignManager : ICampaignManager
    {
        private readonly PosDbContext _context;
        private readonly IMapper _mapper;

        public CampaignManager(PosDbContext posDbContext, IMapper mapper)
        {
            _context = posDbContext;
            _mapper = mapper;
        }

        public CampaignDto Add(CampaignDto dto)
        {
            if (_context.Campaigns.Any(x => x.Code == dto.Code))
                return dto.AddError("Campaign already exists!");

            var entity = _mapper.Map<Campaign>(dto);
            _context.Campaigns.Add(entity);
            _context.SaveChanges();
            dto = _mapper.Map<CampaignDto>(entity);
            return dto.AddSuccess();
        }

        public Result Update(CampaignDto dto)
        {
            var result = new Result();
            var entity = Get(dto.Id);
            if (entity == null)
                return result.AddError("Campaign not found!");

            if (_context.Campaigns.Any(x => x.Code == dto.Code && x.Id != dto.Id))
                return dto.AddError($"The code '{dto.Code}' is already being used!");

            var newEntity = _mapper.Map<Campaign>(dto);
            _context.Campaigns.Update(newEntity);
            _context.SaveChanges();
            return result.AddSuccess();
        }

        public Result Delete(Guid entityId)
        {
            var result = new Result();
            var entity = _context.Campaigns.SingleOrDefault(x => x.Id == entityId);
            if (entity == null)
                return result.AddError("Campaign not found!");

            _context.Campaigns.Remove(entity);
            _context.SaveChanges();
            return result.AddSuccess();
        }

        public CampaignDto Get(Guid id)
        {
            return _mapper.Map<CampaignDto>(_context.Campaigns.AsNoTracking().SingleOrDefault(x => x.Id == id));
        }

        public CampaignDto Get(string code)
        {
            return _mapper.Map<CampaignDto>(_context.Campaigns.AsNoTracking().SingleOrDefault(x => x.Code == code));
        }

        public bool IsExists(Guid entityId)
        {
            return _context.Campaigns.Any(o => o.Id == entityId);
        }

        public PagedResult<CampaignDto> GetWithPaging(int page, int pageSize, Expression<Func<Campaign, bool>> predicate = null)
        {
            if (predicate == null) predicate = entity => true;
            var query = _context.Campaigns.AsNoTracking().Where(predicate);

            var result = new PagedResult<CampaignDto> {CurrentPage = page, PageSize = pageSize, RowCount = query.Count()};
            result.PageCount = (int)Math.Ceiling((double)result.RowCount / pageSize);
 
            var skip = (page - 1) * pageSize;     
            var list = query.Skip(skip).Take(pageSize).ToList();
            foreach (var entity in list)
                result.Results.Add(_mapper.Map<CampaignDto>(entity));

            return result;
        }
    }
}
