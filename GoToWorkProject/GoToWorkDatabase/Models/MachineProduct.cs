namespace GoToWorkDatabase.Models;

internal class MachineProduct
{
    public required string MachineId { get; set; }
    public required string ProductId { get; set; }
    public Machine? Machine { get; set; }
    public Product? Product { get; set; }
}