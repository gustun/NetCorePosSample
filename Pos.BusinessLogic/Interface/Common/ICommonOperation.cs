using System;
using System.Linq.Expressions;
using Pos.BusinessLogic.Dto.Base;
using Pos.Core.Interface;

namespace Pos.BusinessLogic.Interface.Common
{
    public interface ICommonOperation<TDto, TEntity> 
        where TDto : class
        where TEntity : IEntity
    {
        PagedResult<TDto> GetWithPaging(int page, int pageSize, Expression<Func<TEntity, bool>> predicate = null);
        TDto Get(Guid id);
        TDto Add(TDto dto);
        Result Update(TDto dto);
        Result Delete(Guid entityId);
        bool IsExists(Guid entityId);
    }
}
