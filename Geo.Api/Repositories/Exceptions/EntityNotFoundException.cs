namespace Geo.Api.Repositories.Exceptions;

internal sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type type, int id) 
        : base($"Entity '{type.Name}' with id '{id}' not found")
    {
    }
    
    public EntityNotFoundException(Type type) 
        : base($"Entity '{type.Name}' not found")
    {
    }
}