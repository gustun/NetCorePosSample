using Pos.BusinessLogic.Dto;
using Pos.BusinessLogic.Interface.Common;
using Pos.DataAccess.Entities;

namespace Pos.BusinessLogic.Interface
{
    public interface ICampaignManager : ICommonOperation<CampaignDto, Campaign>
    {
        CampaignDto Get(string code);
    }
}
