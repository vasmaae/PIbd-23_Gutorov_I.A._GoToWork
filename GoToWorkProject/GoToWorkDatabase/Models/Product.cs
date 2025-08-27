using System.ComponentModel.DataAnnotations.Schema;

namespace GoToWorkDatabase.Models;

public class Product
{
    public required string Id { get; set; }
    public required string? MachineId { get; set; }
    public required string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public Machine? Machine { get; set; }
    [ForeignKey("ProductId")] public List<DetailProduct>? DetailProducts { get; set; }
}