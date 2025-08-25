using GoToWorkContracts.AdapterContracts.OperationResponses;
using GoToWorkContracts.BindingModels;

namespace GoToWorkContracts.AdapterContracts;

public interface IDetailAdapter
{
    DetailOperationResponse GetList(bool includeDeleted);
    DetailOperationResponse GetElementsByCreationDate(DateTime? from = null, DateTime? to = null);
    DetailOperationResponse GetElement(string data);
    DetailOperationResponse RegisterDetail(DetailBindingModel detailModel);
    DetailOperationResponse ChangeDetailInfo(DetailBindingModel detailModel);
    DetailOperationResponse RemoveDetail(string id);
}