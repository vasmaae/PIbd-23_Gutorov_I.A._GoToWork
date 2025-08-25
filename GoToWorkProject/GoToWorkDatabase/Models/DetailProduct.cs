namespace GoToWorkDatabase.Models;

internal class DetailProduct
{
    public required string ProductId { get; set; }
    public required string DetailId { get; set; }
    public required int Quantity { get; set; }
    public Product? Product { get; set; }
    public Detail? Detail { get; set; }
}