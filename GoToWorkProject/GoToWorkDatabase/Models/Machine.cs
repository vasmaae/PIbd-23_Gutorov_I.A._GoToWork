using System.ComponentModel.DataAnnotations.Schema;
using GoToWorkContracts.Enums;

namespace GoToWorkDatabase.Models;

internal class Machine
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Model { get; set; }
    public required MachineType MachineType { get; set; }
    [ForeignKey("MachineId")] public List<EmployeeMachine>? Employees { get; set; }
    [ForeignKey("MachineId")] public List<MachineProduct>? Products { get; set; }
}