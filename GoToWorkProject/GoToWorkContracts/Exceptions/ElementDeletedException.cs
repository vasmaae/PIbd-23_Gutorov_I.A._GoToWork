namespace GoToWorkContracts.Exceptions;

public class ElementDeletedException(string id)
    : Exception($"Cannot modify a deleted item (id: {id})");