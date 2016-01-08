namespace SSD.Mappers.Interfaces
{
    public interface IDependencyParameterOverride
    {
        string ParameterName { get; set; }

        object ParameterValue { get; set; } 
    }
}