namespace GoToWorkDatabase.Models;

internal class DetailProduction
{
    public required string DetailId { get; set; }
    public required string ProductionId { get; set; }
    public Detail? Detail { get; set; }
    public Production? Production { get; set; }
}