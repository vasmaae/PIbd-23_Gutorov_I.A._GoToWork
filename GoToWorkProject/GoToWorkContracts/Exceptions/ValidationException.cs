namespace GoToWorkContracts.Exceptions;

public class ValidationException(string message) : Exception(message)
{
}