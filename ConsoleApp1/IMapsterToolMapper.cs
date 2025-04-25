namespace ConsoleApp1;

[Mapster.Mapper]
public interface IMapsterToolMapper
{

    public TestDestination MapTo(TestSource source);

    public TestDestinationInner MapTo(TestSourceInner source);

}