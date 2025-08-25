using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IEmailAdapter
{
    Task<EmailOperationResponse> SendEmailAsync(EmailBindingModel emailModel);
}