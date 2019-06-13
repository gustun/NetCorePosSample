using System.Linq;
using Pos.BusinessLogic.Dto.Common;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic.DesignPatterns.Builder
{
    public class ProductFilterBuilder
    {
        private IQueryable<Product> _expression;

        public ProductFilterBuilder(IQueryable<Product> expression)
        {
            _expression = expression;
        }

        public static ProductFilterBuilder Create(IQueryable<Product> query)
        {
            return new ProductFilterBuilder(query);
        }

        public IQueryable<Product> Build()
        {
            return _expression;
        }

        public ProductFilterBuilder SetSearchTerm(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
                _expression = _expression.Where(product => product.Name.Contains(searchTerm) || product.Code.Contains(searchTerm));

            return this;
        }

        public ProductFilterBuilder SetPrice(RangeDto<decimal> priceRange)
        {
            if (priceRange != null)
                _expression = _expression.Where(product => product.Price > priceRange.Min & product.Price < priceRange.Max);

            return this;
        }

        public ProductFilterBuilder SetPaging(int pageIndex, int pageSize)
        {
            _expression = _expression.OrderBy(product => product.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return this;
        }
    }
}
