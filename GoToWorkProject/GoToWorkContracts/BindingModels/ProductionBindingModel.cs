namespace GoToWorkContracts.BindingModels;

public class ProductionBindingModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<DetailProductionBindingModel>? Details { get; set; }
}