using Pos.BusinessLogic.Interface;

namespace Pos.BusinessLogic.DesignPatterns.Factory
{
    public class DiscountAmountCalculator : CampaignCalculator
    {
        public override decimal ApplyCampaign(decimal totalAmount, decimal discountValue)
        {
            return OnAfterApplyCampaing(totalAmount - discountValue);
        }
    }
}
