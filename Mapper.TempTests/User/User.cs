using System.Diagnostics.CodeAnalysis;

namespace Entity.User;

public class UserDto
{
    public Guid Id { get; set; }

    public int? Number { get; set; }

    [AllowNull]
    public string String { get; set; }
}

public class UserDb
{
    public Guid Id { get; set; }

    public int? Number { get; set; }

    [AllowNull]
    public string String { get; set; }
}


