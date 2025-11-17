using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class IncrementalValueProviderExtensions
{
    public static IncrementalValuesProvider<TSource> WhereNotNull<TSource>(this IncrementalValuesProvider<TSource?> source)
        where TSource : class
    {
        return source.Where(x => x is not null)
            .Select((x, _) => x!);
    }

    public static IncrementalValuesProvider<TSource> WhereNotNull<TSource>(this IncrementalValuesProvider<TSource?> source)
        where TSource : struct
    {
        return source.Where(x => x is not null)
            .Select((x, _) => x!.Value);
    }

    public static IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3)> Combine<TItem1, TItem2, TItem3>(
        this IncrementalValueProvider<TItem1> provider1,
        IncrementalValueProvider<TItem2> provider2,
        IncrementalValueProvider<TItem3> provider3
    )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Select((tuple, _) => (tuple.Left.Left, tuple.Left.Right, tuple.Right));
    }

    public static IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4)> Combine<TItem1, TItem2, TItem3, TItem4>(
        this IncrementalValueProvider<TItem1> provider1,
        IncrementalValueProvider<TItem2> provider2,
        IncrementalValueProvider<TItem3> provider3,
        IncrementalValueProvider<TItem4> provider4
    )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Select((tuple, _) => (tuple.Left.Left.Left, tuple.Left.Left.Right, tuple.Left.Right, tuple.Right));
    }

    public static IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4, TItem5 Item5)>
        Combine<TItem1, TItem2, TItem3, TItem4, TItem5>(
            this IncrementalValueProvider<TItem1> provider1,
            IncrementalValueProvider<TItem2> provider2,
            IncrementalValueProvider<TItem3> provider3,
            IncrementalValueProvider<TItem4> provider4,
            IncrementalValueProvider<TItem5> provider5
        )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Combine(provider5)
            .Select((tuple, _) => (tuple.Left.Left.Left.Left, tuple.Left.Left.Left.Right, tuple.Left.Left.Right, tuple.Left.Right, tuple.Right));
    }

    public static IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4, TItem5 Item5, TItem6 Item6)>
        Combine<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6>(
            this IncrementalValueProvider<TItem1> provider1,
            IncrementalValueProvider<TItem2> provider2,
            IncrementalValueProvider<TItem3> provider3,
            IncrementalValueProvider<TItem4> provider4,
            IncrementalValueProvider<TItem5> provider5,
            IncrementalValueProvider<TItem6> provider6
        )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Combine(provider5)
            .Combine(provider6)
            .Select((tuple, _) => (tuple.Left.Left.Left.Left.Left, tuple.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Right, tuple.Left.Left.Right,
                tuple.Left.Right, tuple.Right)
            );
    }

    public static IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4, TItem5 Item5, TItem6 Item6, TItem7 Item7)>
        Combine<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7>(
            this IncrementalValueProvider<TItem1> provider1,
            IncrementalValueProvider<TItem2> provider2,
            IncrementalValueProvider<TItem3> provider3,
            IncrementalValueProvider<TItem4> provider4,
            IncrementalValueProvider<TItem5> provider5,
            IncrementalValueProvider<TItem6> provider6,
            IncrementalValueProvider<TItem7> provider7
        )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Combine(provider5)
            .Combine(provider6)
            .Combine(provider7)
            .Select((tuple, _) => (tuple.Left.Left.Left.Left.Left.Left, tuple.Left.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Left.Right,
                tuple.Left.Left.Left.Right, tuple.Left.Left.Right, tuple.Left.Right, tuple.Right)
            );
    }

    public static IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4, TItem5 Item5, TItem6 Item6, TItem7 Item7, TItem8 Item8)>
        Combine<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8>(
            this IncrementalValueProvider<TItem1> provider1,
            IncrementalValueProvider<TItem2> provider2,
            IncrementalValueProvider<TItem3> provider3,
            IncrementalValueProvider<TItem4> provider4,
            IncrementalValueProvider<TItem5> provider5,
            IncrementalValueProvider<TItem6> provider6,
            IncrementalValueProvider<TItem7> provider7,
            IncrementalValueProvider<TItem8> provider8
        )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Combine(provider5)
            .Combine(provider6)
            .Combine(provider7)
            .Combine(provider8)
            .Select((tuple, _) => (tuple.Left.Left.Left.Left.Left.Left.Left, tuple.Left.Left.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Left.Left.Right,
                tuple.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Right, tuple.Left.Left.Right, tuple.Left.Right, tuple.Right)
            );
    }

    public static
        IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4, TItem5 Item5, TItem6 Item6, TItem7 Item7, TItem8 Item8, TItem9 Item9)>
        Combine<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9>(
            this IncrementalValueProvider<TItem1> provider1,
            IncrementalValueProvider<TItem2> provider2,
            IncrementalValueProvider<TItem3> provider3,
            IncrementalValueProvider<TItem4> provider4,
            IncrementalValueProvider<TItem5> provider5,
            IncrementalValueProvider<TItem6> provider6,
            IncrementalValueProvider<TItem7> provider7,
            IncrementalValueProvider<TItem8> provider8,
            IncrementalValueProvider<TItem9> provider9
        )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Combine(provider5)
            .Combine(provider6)
            .Combine(provider7)
            .Combine(provider8)
            .Combine(provider9)
            .Select((tuple, _) => (tuple.Left.Left.Left.Left.Left.Left.Left.Left, tuple.Left.Left.Left.Left.Left.Left.Left.Right,
                tuple.Left.Left.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Right,
                tuple.Left.Left.Right, tuple.Left.Right, tuple.Right)
            );
    }

    public static
        IncrementalValueProvider<(TItem1 Item1, TItem2 Item2, TItem3 Item3, TItem4 Item4, TItem5 Item5, TItem6 Item6, TItem7 Item7, TItem8 Item8, TItem9 Item9,
            TItem10 Item10)> Combine<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10>(
            this IncrementalValueProvider<TItem1> provider1,
            IncrementalValueProvider<TItem2> provider2,
            IncrementalValueProvider<TItem3> provider3,
            IncrementalValueProvider<TItem4> provider4,
            IncrementalValueProvider<TItem5> provider5,
            IncrementalValueProvider<TItem6> provider6,
            IncrementalValueProvider<TItem7> provider7,
            IncrementalValueProvider<TItem8> provider8,
            IncrementalValueProvider<TItem9> provider9,
            IncrementalValueProvider<TItem10> provider10
        )
    {
        return provider1.Combine(provider2)
            .Combine(provider3)
            .Combine(provider4)
            .Combine(provider5)
            .Combine(provider6)
            .Combine(provider7)
            .Combine(provider8)
            .Combine(provider9)
            .Combine(provider10)
            .Select((tuple, _) => (tuple.Left.Left.Left.Left.Left.Left.Left.Left.Left, tuple.Left.Left.Left.Left.Left.Left.Left.Left.Right,
                tuple.Left.Left.Left.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Left.Left.Right,
                tuple.Left.Left.Left.Left.Right, tuple.Left.Left.Left.Right, tuple.Left.Left.Right, tuple.Left.Right, tuple.Right)
            );
    }
}
