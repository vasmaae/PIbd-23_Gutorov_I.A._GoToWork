using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class EmployeeDataModel : IValidation
{
    public string Id { get; set; }
    public string FullName { get; set; }

    public EmployeeDataModel() { }

    public EmployeeDataModel(string id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a Guid");
        if (FullName.IsEmpty())
            throw new ValidationException("Field FullName is empty");
    }
}