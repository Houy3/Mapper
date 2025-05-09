using System.Text;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.Builder;

public class TextBuilder
{
    public static readonly string NewLine = new StringBuilder().AppendLine().ToString();

    public const string BlockStartSymbol = "{";
    public const string BlockEndSymbol = "}";


    private readonly StringBuilder Text = new();

    public string Str => Text.ToString();

    private string Offset = "";


    public TextBuilder Append(params string?[] textList)
    {
        foreach (var text in textList) 
            Text.Append(text ?? string.Empty); 
        return this;
    }

    public TextBuilder Append(object obj)
        => Append(obj.ToString());

    public TextBuilder Append(Action<TextBuilder> action)
    {
        action(this);
        return this;
    }

    public TextBuilder AppendLine(params string?[] textList)
    {
        Append(textList);
        Text.AppendLine().Append(Offset);
        return this;
    }

    public TextBuilder AppendLine<T>(Action<TextBuilder, T> action, EquatableArrayWrap<T> list)
        where T : notnull
    {
        var length = list.Length;
        for (int i = 0; i < length; i++)
        {
            action(this, list[i]);
            AppendLine();
        }
        return this;
    }


    public TextBuilder AppendJoin<T>(EquatableArrayWrap<T> enumerable, string? separator = null)
        where T : notnull
    {
        var list = enumerable.ToArray();
        var length = list.Length;
        for (int i = 0; i < length; i++)
        {
            Append(list[i]);
            if (i != length - 1)
                Append(separator);
        }
        return this;
    }

    public TextBuilder AppendLineJoin<T>(Action<TextBuilder, T> action, EquatableArrayWrap<T> list, string? separator = null)
        where T : notnull
    {
        var length = list.Length;
        for (int i = 0; i < length; i++)
        {
            action(this, list[i]);
            if (i != length - 1)
                AppendLine(separator);
        }
        return this;
    }


    public TextBuilder StartOffset()
    {
        Offset += "\t";
        return this;
    }

    public TextBuilder EndOffset()
    {
        Offset = Offset.Length > 0 ? Offset.Substring(1) : Offset;
        return this;
    }

    public TextBuilder StartBlock(string blockStartSymbol = BlockStartSymbol)
    {
        StartOffset();
        return AppendLine(blockStartSymbol);
    }

    public TextBuilder EndBlock(string blockEndSymbol = BlockEndSymbol)
    {
        EndOffset();
        return AppendLine().Append(blockEndSymbol);
    }

    public TextBuilder AppendBlock(Action<TextBuilder> action, string blockStartSymbol = BlockStartSymbol, string blockEndSymbol = BlockEndSymbol)
    {
        StartBlock(blockStartSymbol);
        Append(action);
        return EndBlock(blockEndSymbol);
    }

    public override string ToString()
    {
        return Text.ToString();
    }
}
