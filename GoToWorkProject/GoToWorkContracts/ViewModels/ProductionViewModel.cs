using System.Collections.Generic;

namespace GoToWorkContracts.ViewModels;

public class ProductionViewModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required List<DetailProductionViewModel>? Details { get; set; }
    public required List<WorkshopViewModel>? Workshops { get; set; }
}