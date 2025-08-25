using GoToWorkContracts.DataModels;

namespace GoToWorkContracts.StoragesContracts;

public interface IDetailStorageContract
{
    public List<DetailDataModel> GetList(DateTime? startDate = null, DateTime? endDate = null);
    DetailDataModel? GetElementById(string id);
    DetailDataModel? GetElementByName(string name);
    void AddElement(DetailDataModel detailDataModel);
    void UpdElement(DetailDataModel detailDataModel);
    void DelElement(string id);
}