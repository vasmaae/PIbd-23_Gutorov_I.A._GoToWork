using GoToWorkContracts.Enums;
using System.Collections.Generic;

namespace GoToWorkContracts.ViewModels;

public class MachineViewModel
{
    public required string Id { get; set; }
    public required string Model { get; set; }
    public MachineType Type { get; set; }
    public required List<EmployeeMachineViewModel>? Employees { get; set; }
}