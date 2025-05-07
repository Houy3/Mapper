namespace DumbFramework;

public abstract class SimpleBaseMapper<TDto, TDb>
{
    public abstract TDto ToDto(TDb source);
}