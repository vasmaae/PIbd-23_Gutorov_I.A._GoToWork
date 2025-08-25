using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IDetailBusinessLogicContract
{
    List<DetailDataModel> GetAllDetails();
    DetailDataModel GetDetailByData(string data);
    void InsertDetail(DetailDataModel detail);
    void UpdateDetail(DetailDataModel detail);
    void DeleteDetail(string id);
}