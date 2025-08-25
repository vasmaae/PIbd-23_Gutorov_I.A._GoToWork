using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.BusinessLogicContracts;

public interface IDetailBusinessLogicContract
{
    List<DetailDataModel> GetAllDetails();
    List<DetailDataModel> GetDetailsByCreationDate(DateTime from, DateTime to);
    DetailDataModel GetDetailByData(string data);
    void InsertDetail(DetailDataModel detail);
    void UpdateDetail(DetailDataModel detail);
    void DeleteDetail(string id);
}