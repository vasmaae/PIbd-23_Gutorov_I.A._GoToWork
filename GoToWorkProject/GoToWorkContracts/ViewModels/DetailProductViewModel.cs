namespace GoToWorkContracts.ViewModels;

public class DetailProductViewModel
{
    public required string ProductId { get; set; }
    public required string DetailId { get; set; }
    public required string DetailName { get; set; }
    public int Quantity { get; set; }
}