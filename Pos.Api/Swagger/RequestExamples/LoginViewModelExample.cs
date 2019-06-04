using Pos.Api.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace Pos.Api.Swagger.RequestExamples
{
    public class LoginViewModelExample : IExamplesProvider<LoginViewModel>
    {
        public LoginViewModel GetExamples()
        {
            return new LoginViewModel
            {
                UserName = "admin",
                Password =  "1"
            };
        }
    }
}
