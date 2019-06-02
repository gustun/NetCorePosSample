using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Interface.Common;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic.Interface
{
    public interface IProductManager : ICommonOperation<ProductDto, Product>
    {
        ProductDto Get(string code);
    }
}
