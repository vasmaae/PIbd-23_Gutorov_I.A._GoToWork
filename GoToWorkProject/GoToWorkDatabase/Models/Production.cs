using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Production
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkshopId { get; set; }
    public Workshop? Workshop { get; set; }
    [ForeignKey("ProductionId")] public List<DetailProduction>? Details { get; set; }
}