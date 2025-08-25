using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

internal class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string MachineId { get; set; }
    public required string Name { get; set; }
    public required DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public Machine? Machine { get; set; }
    [ForeignKey("ProductId")] public List<DetailProduct>? Details { get; set; }
    [ForeignKey("ProductId")] public List<MachineProduct>? Products { get; set; }
}