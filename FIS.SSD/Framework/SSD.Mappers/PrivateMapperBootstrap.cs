using AutoMapper;

namespace SSD.Mappers
{
    public abstract class PrivateMapperBootstrap
    {
        public abstract void CreateMaps(IConfiguration configuration);
    }
}