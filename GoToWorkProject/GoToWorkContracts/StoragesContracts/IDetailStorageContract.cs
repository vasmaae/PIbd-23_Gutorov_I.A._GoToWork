using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IDetailStorageContract
{
    List<DetailDataModel> GetList(string? userId = null);
    DetailDataModel? GetElementById(string id);
    DetailDataModel? GetElementByName(string name);
    void AddElement(DetailDataModel detailDataModel);
    void UpdElement(DetailDataModel detailDataModel);
    void DelElement(string id);
}