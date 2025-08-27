using System.ComponentModel.DataAnnotations.Schema;
using GoToWorkContracts.Enums;

namespace GoToWorkDatabase.Models;

public class Machine
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Model { get; set; }
    public MachineType MachineType { get; set; }
    [ForeignKey("MachineId")] public List<EmployeeMachine>? EmployeeMachines { get; set; }
    [ForeignKey("MachineId")] public List<Product>? Products { get; set; }
}