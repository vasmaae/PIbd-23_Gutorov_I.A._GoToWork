using System;
using System.Collections.Generic;

namespace GoToWorkContracts.ViewModels;

public class ProductViewModel
{
    public required string Id { get; set; }
    public required string? MachineId { get; set; }
    public required string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public required string? MachineName { get; set; }
    public required List<DetailProductViewModel>? Details { get; set; }
}