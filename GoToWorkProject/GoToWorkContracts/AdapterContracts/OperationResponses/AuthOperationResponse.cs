using GoToWorkContracts.Infrastructure;
using GoToWorkContracts.ViewModels;

namespace GoToWorkContracts.AdapterContracts.OperationResponses
{
    public class AuthOperationResponse : OperationResponse
    {
        public static AuthOperationResponse OK(TokenViewModel data) =>
            OK<AuthOperationResponse, TokenViewModel>(data);

        public static AuthOperationResponse BadRequest(string message) =>
            BadRequest<AuthOperationResponse>(message);
        
        public static AuthOperationResponse Unauthorized(string message) =>
            Unauthorized<AuthOperationResponse>(message);
    }
}
