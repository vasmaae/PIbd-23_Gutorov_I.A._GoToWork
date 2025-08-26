using GoToWorkContracts.Enums;

namespace GoToWorkContracts.BindingModels;

public class MachineBindingModel
{
    public string? Id { get; set; }
    public string? Model { get; set; }
    public MachineType Type { get; set; }
    public List<EmployeeMachineBindingModel>? Employees { get; set; }
    public List<ProductBindingModel>? Products { get; set; }
}