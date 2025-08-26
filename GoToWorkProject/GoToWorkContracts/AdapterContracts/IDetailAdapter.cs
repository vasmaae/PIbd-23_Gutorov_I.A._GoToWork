using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IDetailAdapter
{
    DetailOperationResponse GetList();
    DetailOperationResponse GetElementsByCreationDate(DateTime? from = null, DateTime? to = null);
    DetailOperationResponse GetElement(string data);
    DetailOperationResponse CreateDetail(DetailBindingModel detailModel);
    DetailOperationResponse UpdateDetail(DetailBindingModel detailModel);
    DetailOperationResponse DeleteDetail(string id);
}