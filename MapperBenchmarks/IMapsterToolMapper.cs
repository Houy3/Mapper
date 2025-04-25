using static MapperBenchmarks.MapperTest;

namespace MapperBenchmarks;

[Mapster.Mapper]
public interface IMapsterToolMapper
{

    public TestDestination MapTo(TestSource source);

}