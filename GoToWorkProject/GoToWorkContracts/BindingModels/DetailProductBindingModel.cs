namespace GoToWorkContracts.BindingModels;

public class DetailProductBindingModel
{
    public string? DetailId { set; get; }
    public string? ProductId { get; set; }
    public int Quantity { get; set; }
}