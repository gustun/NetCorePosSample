using Pos.Contracts;

namespace Pos.BusinessLogic.DesignPatterns.Factory
{
    public class DiscountRatioCalculator : CampaignCalculator
    {
        public override decimal ApplyCampaign(decimal totalAmount, decimal discountValue)
        {
            return OnAfterApplyCampaing(totalAmount - (totalAmount * discountValue / 100));
        }
    }
}
