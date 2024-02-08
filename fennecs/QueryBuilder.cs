// SPDX-License-Identifier: MIT

// ReSharper disable MemberCanBePrivate.Global
namespace fennecs;

public class QueryBuilder
{
    internal readonly Archetypes Archetypes;
    protected readonly Mask Mask = MaskPool.Rent();

    internal QueryBuilder(Archetypes archetypes)
    {
        Archetypes = archetypes;
    }

    protected QueryBuilder Has<T>(Identity target = default)
    {
        var typeExpression = TypeExpression.Create<T>(target);
        Mask.Has(typeExpression);
        return this;
    }


    protected QueryBuilder Has<T>(Type type)
    {
        var identity = new Identity(type);
        var typeExpression = TypeExpression.Create<T>(identity);
        Mask.Has(typeExpression);
        return this;
    }


    protected QueryBuilder Not<T>(Identity target = default)
    {
        Mask.Not(TypeExpression.Create<T>(target));
        return this;
    }


    protected QueryBuilder Not<T>(Type type)
    {
        var identity = new Identity(type);
        Mask.Not(TypeExpression.Create<T>(identity));
        return this;
    }


    protected QueryBuilder Any<T>(Identity target = default)
    {
        Mask.Any(TypeExpression.Create<T>(target));
        return this;
    }
    
    
    protected QueryBuilder Any<T>(Type type)
    {
        var identity = new Identity(type);
        Mask.Any(TypeExpression.Create<T>(identity));
        return this;
    }
}

public sealed class QueryBuilder<C> : QueryBuilder
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C>(archetypes, mask, matchingTables);


    internal QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C>();
    }

    
    public new QueryBuilder<C> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C> Has<T>(Type type)
    {
        return (QueryBuilder<C>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C> Not<T>(Type type)
    {
        return (QueryBuilder<C>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C> Any<T>(Type type)
    {
        return (QueryBuilder<C>)base.Any<T>(type);
    }

    
    public Query<C> Build()
    {
        return (Query<C>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2> : QueryBuilder where C2 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>();
    }

    
    public new QueryBuilder<C1, C2> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2>)base.Any<T>(type);
    }

    
    public Query<C1, C2> Build()
    {
        return (Query<C1, C2>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2, C3> : QueryBuilder
    where C1 : struct
    where C2 : struct
    where C3 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>();
    }

    
    public new QueryBuilder<C1, C2, C3> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3> Build()
    {
        return (Query<C1, C2, C3>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2, C3, C4> : QueryBuilder
    where C1 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>();
    }

    
    public new QueryBuilder<C1, C2, C3, C4> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3, C4> Build()
    {
        return (Query<C1, C2, C3, C4>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2, C3, C4, C5> : QueryBuilder
    where C1 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>();
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3, C4, C5> Build()
    {
        return (Query<C1, C2, C3, C4, C5>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

/*
public sealed class QueryBuilder<C1, C2, C3, C4, C5, C6> : QueryBuilder
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>();
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3, C4, C5, C6> Build()
    {
        return (Query<C1, C2, C3, C4, C5, C6>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2, C3, C4, C5, C6, C7> : QueryBuilder
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6, C7>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>();
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3, C4, C5, C6, C7> Build()
    {
        return (Query<C1, C2, C3, C4, C5, C6, C7>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> : QueryBuilder
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6, C7, C8>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>();
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3, C4, C5, C6, C7, C8> Build()
    {
        return (Query<C1, C2, C3, C4, C5, C6, C7, C8>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}

public sealed class QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> : QueryBuilder
    where C1 : struct
    where C2 : struct
    where C3 : struct
    where C4 : struct
    where C5 : struct
    where C6 : struct
    where C7 : struct
    where C8 : struct
    where C9 : struct
{
    private static readonly Func<Archetypes, Mask, List<Table>, Query> CreateQuery =
        (archetypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5, C6, C7, C8, C9>(archetypes, mask, matchingTables);

    
    public QueryBuilder(Archetypes archetypes) : base(archetypes)
    {
        Has<C1>().Has<C2>().Has<C3>().Has<C4>().Has<C5>().Has<C6>().Has<C7>().Has<C8>().Has<C9>();
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Has<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Has<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Has<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Has<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Not<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Not<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Not<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Not<T>(type);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Any<T>(Identity target = default)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Any<T>(target);
    }

    
    public new QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9> Any<T>(Type type)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5, C6, C7, C8, C9>)base.Any<T>(type);
    }

    
    public Query<C1, C2, C3, C4, C5, C6, C7, C8, C9> Build()
    {
        return (Query<C1, C2, C3, C4, C5, C6, C7, C8, C9>)Archetypes.GetQuery(Mask, CreateQuery);
    }
}
*/