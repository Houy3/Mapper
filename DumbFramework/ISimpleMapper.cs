namespace DumbFramework;

public interface ISimpleMapper<TDto, TService>
{
    TService ToService(TDto source);
}
