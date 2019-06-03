namespace Pos.Contracts
{
    public interface ICampaignCalculator
    {
        decimal ApplyCampaign(decimal totalAmount, decimal discountValue);
    }

    public abstract class CampaignCalculator : ICampaignCalculator
    {
        public abstract decimal ApplyCampaign(decimal totalAmount, decimal discountValue);
        public decimal OnAfterApplyCampaing(decimal newTotalAmount) => newTotalAmount < 0 ? 0 : newTotalAmount;
    }
}
