using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IUserAdapter
{
    UserOperationResponse GetList();
    UserOperationResponse GetElement(string data);
    UserOperationResponse CreateUser(UserBindingModel userModel);
    UserOperationResponse UpdateUser(UserBindingModel userModel);
    UserOperationResponse DeleteUser(string id);
    UserOperationResponse Register(UserRegisterBindingModel model);
    AuthOperationResponse Login(UserLoginBindingModel model);
}