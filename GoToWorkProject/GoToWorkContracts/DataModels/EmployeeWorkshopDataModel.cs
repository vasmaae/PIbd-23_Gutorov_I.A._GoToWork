using GoToWorkContracts.Exceptions;
using GoToWorkContracts.Extensions;
using GoToWorkContracts.Infrastructure;

namespace GoToWorkContracts.DataModels;

public class EmployeeWorkshopDataModel : IValidation
{
    public string EmployeeId { get; set; }
    public string WorkshopId { get; set; }
    public string EmployeeName => _employee?.FullName ?? string.Empty;
    public string WorkshopName => _workshop?.Address ?? string.Empty;
    private readonly EmployeeDataModel? _employee;
    private readonly WorkshopDataModel? _workshop;

    public EmployeeWorkshopDataModel(string employeeId, string workshopId)
    {
        EmployeeId = employeeId;
        WorkshopId = workshopId;
    }

    public EmployeeWorkshopDataModel(string employeeId, string workshopId, EmployeeDataModel? employee,
        WorkshopDataModel? workshop) : this(employeeId, workshopId)
    {
        _employee = employee;
        _workshop = workshop;
    }

    public EmployeeWorkshopDataModel()
    {
    }

    public void Validate()
    {
        if (EmployeeId.IsEmpty())
            throw new ValidationException("Field EmployeeId is empty");
        if (!EmployeeId.IsGuid())
            throw new ValidationException("The value in the field EmployeeId is not a Guid");
        if (WorkshopId.IsEmpty())
            throw new ValidationException("Field WorkshopId is empty");
        if (!WorkshopId.IsGuid())
            throw new ValidationException("The value in the field WorkshopId is not a Guid");
    }
}