using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace MapperBenchmarks;

[MemoryDiagnoser]
public class MapperTest
{
    #region Test Types

    public class TestSource()
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        [AllowNull]
        public string String { get; set; }

        [AllowNull]
        public TestSourceInner Inner { get; set; }

    }

    public class TestSourceInner()
    {

        [AllowNull]
        public int[] NumberList { get; set; }

        [AllowNull]
        public string String1 { get; set; }

        [AllowNull]
        public string String2 { get; set; }

    }

    public class TestDestination()
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        [AllowNull]
        public string String { get; set; }

        [AllowNull]
        public TestDestinationInner Inner { get; set; }

    }

    public class TestDestinationInner()
    {

        [AllowNull]
        public int[] NumberList { get; set; }

        [AllowNull]
        public string String1 { get; set; }

        [AllowNull]
        public string String2 { get; set; }

    }

    #endregion

    public TestSource TestData = new()
    {
        Id = Guid.NewGuid(),
        Number = 15,
        String = "abracadabra",
        Inner = new()
        {
            NumberList = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
            String1 = "Name",
            String2 = "Code"
        }
    };

    #region Manual

    [Benchmark(Description = "Manual", Baseline = true)]
    public TestDestination WithManual()
        => ManualMapping(TestData);


    private static TestDestination ManualMapping(TestSource source)
    {
        var p3 = source.Inner.NumberList;
        int[] result = new int[p3.Length];
        Array.Copy(p3, 0, result, 0, p3.Length);
        return new()
        {
            Id = source.Id,
            Number = source.Number,
            String = source.String,
            Inner = new()
            {
                NumberList = result,
                String1 = source.Inner.String1,
                String2 = source.Inner.String2,
            }
        };
    }

    #endregion

    #region AutoMapper

    [Benchmark(Description = "AutoMapper")]
    public TestDestination WithAutoMapper()
        => AutoMapper.Map<TestDestination>(TestData);


    public AutoMapper.IMapper AutoMapper = CreateAutoMapper();

    public static AutoMapper.IMapper CreateAutoMapper()
    {
        var config = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TestSource, TestDestination>();
            cfg.CreateMap<TestSourceInner, TestDestinationInner>();
        });
        return config.CreateMapper();
    }

    #endregion

    #region Mapster

    [Benchmark(Description = "Mapster")]
    public TestDestination WithMapster()
        => MapsterMapper.Map<TestDestination>(TestData);


    public MapsterMapper.Mapper MapsterMapper = CreateMapsterMapper();

    public static MapsterMapper.Mapper CreateMapsterMapper()
    {
        var config = new Mapster.TypeAdapterConfig();
        config.NewConfig<TestSource, TestDestination>();
        config.NewConfig<TestSourceInner, TestDestinationInner>();
        return new MapsterMapper.Mapper(config);
    }

    #endregion

    #region MapsterTool

    [Benchmark(Description = "MapsterTool")]
    public TestDestination WithMapsterTool()
        => MapsterToolMapper.MapTo(TestData);

    public IMapsterToolMapper MapsterToolMapper = new MapsterToolMapper();

    #endregion
}

