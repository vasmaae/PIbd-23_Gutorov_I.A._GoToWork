namespace GoToWorkContracts.BindingModels;

public class ProductBindingModel
{
    public string? Id { get; set; }
    public string? MachineId { get; set; }
    public string? Name { get; set; }
    public DateTime CreationDate { get; set; }
    public List<DetailProductBindingModel>? Details { get; set; }
}