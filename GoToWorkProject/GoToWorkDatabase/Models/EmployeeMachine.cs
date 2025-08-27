namespace GoToWorkDatabase.Models;

public class EmployeeMachine
{
    public required string EmployeeId { get; set; }
    public required string MachineId { get; set; }
    public Employee? Employee { get; set; }
    public Machine? Machine { get; set; }
}