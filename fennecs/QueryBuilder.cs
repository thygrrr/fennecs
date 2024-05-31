// SPDX-License-Identifier: MIT

// ReSharper disable MemberCanBePrivate.Global

using fennecs.pools;

namespace fennecs;

/// <summary>
/// Fluent builder interface which compiles Queries via its Build() method.
/// </summary>
/// <para>
/// A QueryBuilder serves to specify inclusion/exclusion criteria for entities and
/// their components, and then compiles them into fast Queries via its Build() method.
/// </para>
/// <para>
/// Example use:
/// <code>
/// <![CDATA[
/// var world = new fennecs.World();
/// 
/// var selectedHealthBars = world.Query<HP, HPBar>()
///     .Has<Selected>()
///     .Any<Player>()
///     .Any<NPC>()
///     .Not<Disabled>()
///     .Compile();
///
/// 
/// selectedHealthBars.For(
///     (ref HP hp, ref HPBar bar) =>
///     {
///         bar.Fill = hp.Cur/hp.Max;
///     });
/// ]]>
/// </code>
/// </para>
/// <remarks>
/// Compilation is reasonably fast, and cached.
/// A Query with the same Mask of criteria will be pulled from the cache if it was already compiled. 
/// You can compile multiple queries from the same builder (adding more criteria as you go).
/// </remarks>
public abstract class QueryBuilder : IDisposable
{
    #region Internals

    internal readonly World World;
    internal readonly Mask Mask = MaskPool.Rent();

    protected private readonly PooledList<TypeExpression> StreamTypes = PooledList<TypeExpression>.Rent();

    internal QueryBuilder(World world)
    {
        World = world;
    }

    protected private void Outputs<T>(Identity target)
    {
        var typeExpression = TypeExpression.Of<T>(target);
        StreamTypes.Add(typeExpression);
        Mask.Has(typeExpression);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Mask.Dispose();
        StreamTypes.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion

    
    #region Public API

    /// <summary>
    /// Disables conflict checks for subsequent Match Expressions.
    /// </summary>
    /// <remarks>
    /// This is useful for programmatically created queries, where duplicate or conflicting Match
    /// Expressions can be intentional or impossible to prevent.
    /// </remarks>
    /// <returns>itself (fluent pattern)</returns>
    public virtual QueryBuilder Unchecked()
    {
        Mask.safety = false;
        return this;
    }
    
    /// <inheritdoc cref="Compile"/>
    [Obsolete("Use Compile() or Unique() instead.")]
    public abstract Query Build();
    
    
    /// <summary>
    /// Builds (compiles) the Query from the current state of the QueryBuilder.
    /// </summary>
    /// <remarks>
    /// This method is covariant, so you will get the appropriate stream Query subclass
    /// depending on the Stream Types (type parameters) you passed to <see cref="fennecs.World.Query{C}()"/>
    /// or any of its overloads.
    /// </remarks>
    /// <returns>compiled query (you can compile more than one query from the same builder)</returns>
    public abstract Query Compile();
    
    
    /// <summary>
    /// Compiles the query, but does not add it to the internal cache.
    /// </summary>
    /// <remarks>
    /// This is useful for queries with frequently changing filter states, where
    /// other queries operating on the same sets of entities should not inherit this state.
    /// </remarks>
    /// <returns>compiled query (you can compile more than one query from the same builder)</returns>
    public abstract Query Unique();
    
    
    /// <summary>
    /// Include only Entities that have the given Component or Relation.
    /// </summary>
    /// <param name="target">relation target (defaults to no target = Plain Component)</param>
    /// <typeparam name="T">component type</typeparam>
    /// <returns>itself (fluent pattern)</returns>
    /// <exception cref="InvalidOperationException">if the StreamTypes already cover this</exception>
    public virtual QueryBuilder Has<T>(Identity target)
    {
        var typeExpression = TypeExpression.Of<T>(target);
        
        Mask.Has(typeExpression);
        return this;
    }

    /// <summary>
    /// Include only Entities that have the given ObjectLink.
    /// </summary>
    /// <param name="target">relation target</param>
    /// <typeparam name="T">component type</typeparam>
    /// <returns>itself (fluent pattern)</returns>
    /// <exception cref="InvalidOperationException">if the StreamTypes already cover this</exception>
    public virtual QueryBuilder Has<T>(T target) where T : class
    {
        Mask.Has(TypeExpression.Of<T>(Identity.Of(target)));
        return this;
    }

    
    /// <summary>
    /// Exclude all Entities that have the given Component or Relation.
    /// </summary>
    /// <param name="target">relation target (defaults to no target = Plain Component)</param>
    /// <typeparam name="T">component type</typeparam>
    /// <returns>itself (fluent pattern)</returns>
    /// <exception cref="InvalidOperationException">if the StreamTypes already cover this</exception>
    public virtual QueryBuilder Not<T>(Identity target)
    {
        Mask.Not(TypeExpression.Of<T>(target));
        return this;
    }

    /// <inheritdoc cref="Not{T}(fennecs.Identity)"/>
    public virtual QueryBuilder Not<T>() => Not<T>(Match.Plain);

    /// <inheritdoc cref="Has{T}(fennecs.Identity)"/>
    public virtual QueryBuilder Has<T>() => Has<T>(Match.Plain);

    /// <inheritdoc cref="Any{T}(fennecs.Identity)"/>
    public virtual QueryBuilder Any<T>() => Any<T>(Match.Plain);


    /// <summary>
    /// Exclude all Entities that have the given ObjectLink.
    /// </summary>
    /// <param name="target">link target</param>
    /// <typeparam name="T">component type</typeparam>
    /// <returns>itself (fluent pattern)</returns>
    /// <exception cref="InvalidOperationException">if the StreamTypes already cover this</exception>
    public virtual QueryBuilder Not<T>(T target) where T : class
    {
        var typeExpression = TypeExpression.Of<T>(Identity.Of(target));

        Mask.Not(typeExpression);
        return this;
    }


    /// <summary>
    /// Include Entities that have the given Component or Relation, or any other Relation that is
    /// givein in other <see cref="Any{T}(fennecs.Identity)"/> calls.
    /// </summary>
    /// <param name="target">relation target (defaults to no target = Plain Component)</param>
    /// <typeparam name="T">component type</typeparam>
    /// <returns>itself (fluent pattern)</returns>
    /// <exception cref="InvalidOperationException">if the StreamTypes already cover this</exception>
    public virtual QueryBuilder Any<T>(Identity target)
    {
        Mask.Any(TypeExpression.Of<T>(target));
        return this;
    }


    /// <summary>
    /// Include Entities that have the given Object Link, or any other Object Link that is
    /// given in other <see cref="Any{T}(T)"/> calls.
    /// </summary>
    /// <param name="target">link target</param>
    /// <typeparam name="T">component type</typeparam>
    /// <returns>itself (fluent pattern)</returns>
    /// <exception cref="InvalidOperationException">if the StreamTypes already cover this</exception>
    public virtual QueryBuilder Any<T>(T target) where T : class
    {
        Mask.Any(TypeExpression.Of<T>(Identity.Of(target)));
        return this;
    }
    
    #endregion

}

/// <inheritdoc />
public sealed class QueryBuilder<C1> : QueryBuilder where C1 : notnull
{
    private static readonly Func<World, List<TypeExpression>, Mask, List<Archetype>, Query> CreateQuery =
        (world, streamTypes, mask, matchingTables) => new Query<C1>(world, streamTypes, mask, matchingTables);


    internal QueryBuilder(World world, Identity match) : base(world)
    {
        Outputs<C1>(match);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1> Unchecked()
    {
        return (QueryBuilder<C1>) base.Unchecked();
    }

    
    /// <inheritdoc />
    public override Query<C1> Compile() 
    {
        return (Query<C1>) World.CacheQuery(StreamTypes, Mask, CreateQuery);
    }

    
    /// <inheritdoc />
    [Obsolete("Use Compile() or Unique() instead.")]
    public override Query<C1> Build() => Compile();


    /// <inheritdoc />
    public override Query<C1> Unique()
    {
        return (Query<C1>) World.CompileQuery(StreamTypes, Mask, CreateQuery);
    }
    
    
    /// <inheritdoc />
    public override QueryBuilder<C1> Has<T>(Identity target)
    {
        return (QueryBuilder<C1>) base.Has<T>(target);
    }

    
    /// <inheritdoc />
    public override QueryBuilder<C1> Has<T>(T target) where T : class
    {
        return (QueryBuilder<C1>) base.Has(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1> Not<T>(Identity target)
    {
        return (QueryBuilder<C1>) base.Not<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1> Not<T>(T target) where T : class
    {
        return (QueryBuilder<C1>) base.Not(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1> Any<T>(Identity target)
    {
        return (QueryBuilder<C1>) base.Any<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1> Any<T>(T target) where T : class
    {
        return (QueryBuilder<C1>) base.Any(target);
    }
    
    
    /// <inheritdoc />
    public override QueryBuilder<C1> Has<T>() => Has<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1> Not<T>() => Not<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1> Any<T>() => Any<T>(Match.Plain);
    
}

/// <inheritdoc />
public sealed class QueryBuilder<C1, C2> : QueryBuilder where C2 : notnull where C1 : notnull
{
    private static readonly Func<World, List<TypeExpression>, Mask, List<Archetype>, Query> CreateQuery =
        (world, streamTypes, mask, matchingTables) => new Query<C1, C2>(world, streamTypes, mask, matchingTables);


    internal QueryBuilder(World world, Identity match1, Identity match2) : base(world)
    {
        Outputs<C1>(match1);
        Outputs<C2>(match2);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Unchecked()
    {
        return (QueryBuilder<C1, C2>) base.Unchecked();
    }
   

    /// <inheritdoc />
    public override Query<C1, C2> Compile() 
    {
        return (Query<C1, C2>) World.CacheQuery(StreamTypes, Mask, CreateQuery);
    }

    
    /// <inheritdoc />
    [Obsolete("Use Compile() or Unique() instead.")]
    public override Query<C1, C2> Build() => Compile();


    /// <inheritdoc />
    public override Query<C1, C2> Unique()
    {
        return (Query<C1, C2>) World.CompileQuery(StreamTypes, Mask, CreateQuery);
    }

    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Has<T>(Identity target)
    {
        return (QueryBuilder<C1, C2>) base.Has<T>(target);
    }

    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Has<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2>) base.Has(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Not<T>(Identity target)
    {
        return (QueryBuilder<C1, C2>) base.Not<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Not<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2>) base.Not(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Any<T>(Identity target)
    {
        return (QueryBuilder<C1, C2>) base.Any<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Any<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2>) base.Any(target);
    }
    
    
    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Has<T>() => Has<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Not<T>() => Not<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2> Any<T>() => Any<T>(Match.Plain);
    
}

/// <inheritdoc />
public sealed class QueryBuilder<C1, C2, C3> : QueryBuilder where C2 : notnull where C3 : notnull where C1 : notnull
{
    private static readonly Func<World, List<TypeExpression>, Mask, List<Archetype>, Query> CreateQuery =
        (world, streamTypes, mask, matchingTables) => new Query<C1, C2, C3>(world, streamTypes, mask, matchingTables);


    internal QueryBuilder(World world, Identity match1, Identity match2, Identity match3) : base(world)
    {
        Outputs<C1>(match1);
        Outputs<C2>(match2);
        Outputs<C3>(match3);
    }



    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Unchecked()
    {
        return (QueryBuilder<C1, C2, C3>) base.Unchecked();
    }
    
    
    /// <inheritdoc />
    public override Query<C1, C2, C3> Compile() 
    {
        return (Query<C1, C2, C3>) World.CacheQuery(StreamTypes, Mask, CreateQuery);
    }

    
    /// <inheritdoc />
    [Obsolete("Use Compile() or Unique() instead.")]
    public override Query<C1, C2, C3> Build() => Compile();


    /// <inheritdoc />
    public override Query<C1, C2, C3> Unique()
    {
        return (Query<C1, C2, C3>) World.CompileQuery(StreamTypes, Mask, CreateQuery);
    }

    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Has<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3>) base.Has<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Has<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3>) base.Has(target);
    }

    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Not<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3>) base.Not<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Not<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3>) base.Not(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Any<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3>) base.Any<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Any<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3>) base.Any(target);
    }
    
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Has<T>() => Has<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Not<T>() => Not<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3> Any<T>() => Any<T>(Match.Plain);
    
}

/// <inheritdoc />
public sealed class QueryBuilder<C1, C2, C3, C4> : QueryBuilder where C4 : notnull where C3 : notnull where C2 : notnull where C1 : notnull
{
    private static readonly Func<World, List<TypeExpression>, Mask, List<Archetype>, Query> CreateQuery =
        (world, streamTypes, mask, matchingTables) => new Query<C1, C2, C3, C4>(world, streamTypes, mask, matchingTables);


    internal QueryBuilder(World world, Identity match1, Identity match2, Identity match3, Identity match4) : base(world)
    {
        Outputs<C1>(match1);
        Outputs<C2>(match2);
        Outputs<C3>(match3);
        Outputs<C4>(match4);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Unchecked()
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Unchecked();
    }

    /// <inheritdoc />
    public override Query<C1, C2, C3, C4> Compile() 
    {
        return (Query<C1, C2, C3, C4>) World.CacheQuery(StreamTypes, Mask, CreateQuery);
    }

    
    /// <inheritdoc />
    [Obsolete("Use Compile() or Unique() instead.")]
    public override Query<C1, C2, C3, C4> Build() => Compile();


    /// <inheritdoc />
    public override Query<C1, C2, C3, C4> Unique()
    {
        return (Query<C1, C2, C3, C4>) World.CompileQuery(StreamTypes, Mask, CreateQuery);
    }

    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Has<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Has<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Has<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Has(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Not<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Not<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Not<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Not(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Any<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Any<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Any<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3, C4>) base.Any(target);
    }
    
    
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Has<T>() => Has<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Not<T>() => Not<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4> Any<T>() => Any<T>(Match.Plain);
    
}

/// <inheritdoc />
public sealed class QueryBuilder<C1, C2, C3, C4, C5> : QueryBuilder where C5 : notnull where C4 : notnull where C3 : notnull where C2 : notnull where C1 : notnull
{
    private static readonly Func<World, List<TypeExpression>, Mask, List<Archetype>, Query> CreateQuery =
        (world, streamTypes, mask, matchingTables) => new Query<C1, C2, C3, C4, C5>(world, streamTypes, mask, matchingTables);


    internal QueryBuilder(World world, Identity match1, Identity match2, Identity match3, Identity match4, Identity match5) : base(world)
    {
        Outputs<C1>(match1);
        Outputs<C2>(match2);
        Outputs<C3>(match3);
        Outputs<C4>(match4);
        Outputs<C5>(match5);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Unchecked()
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Unchecked();
    }
    
    /// <inheritdoc />
    public override Query<C1, C2, C3, C4, C5> Compile() 
    {
        return (Query<C1, C2, C3, C4, C5>) World.CacheQuery(StreamTypes, Mask, CreateQuery);
    }

    
    /// <inheritdoc />
    [Obsolete("Use Compile() or Unique() instead.")]
    public override Query<C1, C2, C3, C4, C5> Build() => Compile();


    /// <inheritdoc />
    public override Query<C1, C2, C3, C4, C5> Unique()
    {
        return (Query<C1, C2, C3, C4, C5>) World.CompileQuery(StreamTypes, Mask, CreateQuery);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Has<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Has<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Has<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Has(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Not<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Not<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Not<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Not(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Any<T>(Identity target)
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Any<T>(target);
    }


    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Any<T>(T target) where T : class
    {
        return (QueryBuilder<C1, C2, C3, C4, C5>) base.Any(target);
    }
    
    
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Has<T>() => Has<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Not<T>() => Not<T>(Match.Plain);
    /// <inheritdoc />
    public override QueryBuilder<C1, C2, C3, C4, C5> Any<T>() => Any<T>(Match.Plain);
}