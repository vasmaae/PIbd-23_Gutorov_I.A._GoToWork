using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Production
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    [ForeignKey("ProductionId")] public List<DetailProduction>? DetailProductions { get; set; }
    [ForeignKey("ProductionId")] public List<Workshop>? Workshops { get; set; }
}