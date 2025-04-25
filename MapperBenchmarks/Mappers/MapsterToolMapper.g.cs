using System;
using MapperBenchmarks;

namespace MapperBenchmarks
{
    public partial class MapsterToolMapper : IMapsterToolMapper
    {
        public MapperTest.TestDestination MapTo(MapperTest.TestSource p1)
        {
            return p1 == null ? null : new MapperTest.TestDestination()
            {
                Id = p1.Id,
                Number = p1.Number,
                String = p1.String,
                Inner = funcMain1(p1.Inner)
            };
        }
        
        private MapperTest.TestDestinationInner funcMain1(MapperTest.TestSourceInner p2)
        {
            return p2 == null ? null : new MapperTest.TestDestinationInner()
            {
                NumberList = funcMain2(p2.NumberList),
                String1 = p2.String1,
                String2 = p2.String2
            };
        }
        
        private int[] funcMain2(int[] p3)
        {
            if (p3 == null)
            {
                return null;
            }
            int[] result = new int[p3.Length];
            Array.Copy(p3, 0, result, 0, p3.Length);
            return result;
            
        }
    }
}