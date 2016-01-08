namespace SSD.Mappers.Interfaces
{
    public interface IMapper<T, U>
    {
        U Map(T source);
    }
}