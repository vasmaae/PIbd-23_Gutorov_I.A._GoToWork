using System.ComponentModel.DataAnnotations.Schema;
using GoToWorkContracts.Enums;

namespace GoToWorkDatabase.Models;

internal class Detail
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string UserId { get; set; }
    public required string Name { get; set; }
    public required MaterialType Material { get; set; }
    public User? User { get; set; }
    [ForeignKey("DetailId")] public List<DetailProduct>? Products { get; set; }
    [ForeignKey("DetailId")] public List<DetailProduction>? Productions { get; set; }
}