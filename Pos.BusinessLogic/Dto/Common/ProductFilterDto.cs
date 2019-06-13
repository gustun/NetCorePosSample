namespace Pos.BusinessLogic.Dto.Common
{
    public class ProductFilterDto
    {
        public string SearchTerm { get; set; }
        public RangeDto<decimal> Price { get; set; }
    }
}
