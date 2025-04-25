using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1;

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
