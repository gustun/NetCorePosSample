using Pos.BusinessLogic.Dto.Base;

namespace Pos.BusinessLogic.Dto.Common
{
    public class ProductFilterOptionsDto : Result
    {
        public RangeDto<decimal> Price { get; set; }
    }
}
