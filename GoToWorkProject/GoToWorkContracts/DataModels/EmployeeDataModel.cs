using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class EmployeeDataModel(
    string id,
    string fullName)
    : IValidation
{
    public string Id { get; } = id;
    public string FullName { get; } = fullName;

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