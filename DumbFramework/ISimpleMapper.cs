namespace DumbFramework;

public interface ISimpleMapper<TDto, TDb>
{
    TDb ToDb(TDto source);
}
