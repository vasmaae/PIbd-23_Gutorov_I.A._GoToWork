using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class DetailProductDataModel : IValidation
{
    public string ProductId { get; set; }
    public string DetailId { get; set; }
    public int Quantity { get; set; }
    private ProductDataModel? _product;
    private DetailDataModel? _detail;
    public string DetailName => _detail?.Name ?? string.Empty;
    public string ProductName => _product?.Name ?? string.Empty;

    public DetailProductDataModel() { }

    public DetailProductDataModel(string productId, string detailId, int quantity)
    {
        ProductId = productId;
        DetailId = detailId;
        Quantity = quantity;
    }

    public DetailProductDataModel(string productId, string detailId, int quantity, DetailDataModel? detail,
        ProductDataModel? product) : this(productId, detailId, quantity)
    {
        _detail = detail;
        _product = product;
    }

    public void Validate()
    {
        if (ProductId.IsEmpty())
            throw new ValidationException("Field ProductId is empty");
        if (!ProductId.IsGuid())
            throw new ValidationException("The value in the field ProductId is not a Guid");
        if (DetailId.IsEmpty())
            throw new ValidationException("Field DetailId is empty");
        if (!DetailId.IsGuid())
            throw new ValidationException("The value in the field DetailId is not a Guid");
        if (Quantity <= 0)
            throw new ValidationException("Field Quantity is less than or equal to 0");
    }
}