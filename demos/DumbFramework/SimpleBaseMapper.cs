namespace DumbFramework;

public abstract class SimpleBaseMapper<TDto, TService>
{
    public abstract TDto ToDto(TService source);

    public abstract TService ToService(TDto source);
}