namespace DumbFramework;

public interface ISimpleMapper<TDto, TService>
{
    TDto ToDto(TService source);

    TService ToService(TDto source);
}
