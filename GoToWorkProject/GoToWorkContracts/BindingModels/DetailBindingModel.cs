using GoToWorkContracts.Enums;

namespace GoToWorkContracts.BindingModels;

public class DetailBindingModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public MaterialType Material { get; set; }
    public DateTime CreationDate { get; set; }
}