using GoToWorkContracts.Enums;

namespace GoToWorkContracts.ViewModels;

public class DetailViewModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public MaterialType Material { get; set; }
    public DateTime CreationDate { get; set; }
}