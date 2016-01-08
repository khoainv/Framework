namespace SSD.Mappers.Interfaces
{
    public interface IMappingEngine
    {
        TDestination Map<TSource, TDestination>(TSource source);

        TDestination Map<TDestination>(object source); 
    }
}