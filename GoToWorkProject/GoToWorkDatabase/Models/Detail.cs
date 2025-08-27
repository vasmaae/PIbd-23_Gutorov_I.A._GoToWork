using System.ComponentModel.DataAnnotations.Schema;
using GoToWorkContracts.Enums;

namespace GoToWorkDatabase.Models;

public class Detail
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public MaterialType Material { get; set; }
    public DateTime CreationDate { get; set; }
    [ForeignKey("DetailId")] public List<DetailProduct>? DetailProducts { get; set; }
    [ForeignKey("DetailId")] public List<DetailProduction>? DetailProductions { get; set; }
}