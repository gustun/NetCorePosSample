using System;
using Pos.Contracts;
using Pos.Core.Enum;

namespace Pos.BusinessLogic.DesignPatterns.Factory
{

    /// <summary>
    /// Simple example for Factory Pattern.
    /// </summary>
    public static class CampaginCalculatorFactory
    {
        public static ICampaignCalculator Create(EDiscountType discountType)
        {
            switch (discountType)
            {
                case EDiscountType.Amount:
                    return new DiscountAmountCalculator();
                case EDiscountType.Ratio:
                    return new DiscountRatioCalculator();
                default:
                    throw new ArgumentOutOfRangeException(nameof(discountType), discountType, null);
            }
        }
    }
}
