using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.AdapterContracts.OperationResponses
{
    public class EmailOperationResponse : OperationResponse
    {
        public static EmailOperationResponse OK() => OK<EmailOperationResponse, object>(null);
        public static EmailOperationResponse BadRequest(string message) => BadRequest<EmailOperationResponse>(message);
        public static EmailOperationResponse InternalServerError(string message) => InternalServerError<EmailOperationResponse>(message);
    }
}
