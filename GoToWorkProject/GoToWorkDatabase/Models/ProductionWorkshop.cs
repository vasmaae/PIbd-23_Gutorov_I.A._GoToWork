namespace GoToWorkDatabase.Models;

internal class ProductionWorkshop
{
    public required string ProductionId { get; set; }
    public required string WorkshopId { get; set; }
    public Production? Production { get; set; }
    public Workshop? Workshop { get; set; }
}