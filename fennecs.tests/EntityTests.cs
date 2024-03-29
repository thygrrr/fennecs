﻿namespace fennecs.tests;

public class EntityTests(ITestOutputHelper output)
{
    [Fact]
    public void Can_Relate_to_Entity()
    {
        using var world = new World();
        var entity = world.Spawn();
        var target = world.Spawn();
        var builder = new Entity(world, entity);
        builder.AddRelation<int>(target);
        Assert.True(entity.HasRelation<int>(target));
        Assert.False(entity.HasRelation<int>(new Entity(world, new Identity(9001))));
    }


    [Fact]
    public void Can_Relate_to_Entity_with_Data()
    {
        using var world = new World();
        var entity = world.Spawn();
        var target = world.Spawn();
        var builder = new Entity(world, entity);
        builder.AddRelation(target, 123);
        Assert.True(entity.HasRelation<int>(target));
        Assert.False(entity.HasRelation<int>(new Entity(world, new Identity(9001))));
    }


    [Fact]
    public void Entity_has_ToString()
    {
        using var world = new World();
        var entity = world.Spawn();
        var builder = new Entity(world, entity.Id);
        Assert.Equal(entity.ToString(), builder.ToString());

        entity.Add(123);
        entity.AddRelation(world.Spawn(), 7.0f);
        entity.AddLink("hello");
        output.WriteLine(entity.ToString());
        
        world.Despawn(entity);
        output.WriteLine(entity.ToString());
    }


    [Fact]
    public void Entity_Can_Despawn_Itself()
    {
        using var world = new World();
        var entity = world.Spawn();
        entity.Add(123);
        entity.AddRelation(world.Spawn(), 7.0f);
        entity.AddLink("hello");
        entity.Despawn();
        Assert.False(world.IsAlive(entity));
    }

    [Fact]
    public void Entity_Is_Comparable()
    {
        using var world = new World();
        var entity1 = new Entity(null!, new Identity(1));
        var entity2 = new Entity(null!, new Identity(2));
        var entity3 = new Entity(null!, new Identity(3));

        Assert.True(entity1.CompareTo(entity2) < 0);
        Assert.True(entity2.CompareTo(entity3) < 0);
        Assert.True(entity1.CompareTo(entity3) < 0);
    }


    [Fact]
    public void Entity_Is_Equal_Same_Id_Same_World()
    {
        using var world = new World();
        var entity1 = world.Spawn();
        var entity2 = new Entity(world, entity1.Id);
        Assert.Equal(entity1, entity2);
        Assert.True(entity1 == entity2);
        Assert.True(entity2 == entity1);
    }


    [Fact]
    public void Entity_Is_Distinct_Same_Id_Different_World()
    {
        using var world = new World();
        var entity1 = world.Spawn();
        var entity3 = new Entity(null!, entity1.Id);
        Assert.NotEqual(entity1, entity3);
        Assert.True(entity1 != entity3);
        Assert.True(entity3 != entity1);
    }


    [Fact]
    public void Entity_Is_Distinct_Different_Id_Same_World()
    {
        using var world = new World();
        var entity1 = world.Spawn();
        var entity2 = world.Spawn();
        Assert.NotEqual(entity1, entity2);
        Assert.True(entity1 != entity2);
        Assert.True(entity2 != entity1);
    }


    [Fact]
    public void Entity_Is_Distinct_Different_Id_Different_World()
    {
        using var world1 = new World();
        using var world2 = new World();
        var entity1 = world2.Spawn();
        var entity2 = new Entity(null!, new Identity(2));
        Assert.NotEqual(entity1, entity2);
        Assert.True(entity1 != entity2);
        Assert.True(entity2 != entity1);
    }


    [Fact]
    public void Entity_is_Equatable_to_Object()
    {
        using var world = new World();
        var entity1 = world.Spawn();
        var entity2 = new Entity(world, entity1.Id);
        Assert.True(entity1.Equals(entity2));
        Assert.True(entity1.Equals((object) entity2));
        // ReSharper disable once SuspiciousTypeConversion.Global
        Assert.False(entity1.Equals("can't touch this"));
    }


    [Fact]
    public void Entity_Is_Hashable()
    {
        using var world = new World();
        var entity1 = world.Spawn();
        var entity2 = world.Spawn();
        var entity3 = new Entity(world, entity1.Id);
        var entity4 = new Entity(world, entity2.Id);
        var set = new HashSet<Entity> {entity1, entity2, entity3, entity4};
        Assert.Equal(2, set.Count);
    }


    [Fact]
    public void Entity_Decays_to_Identity()
    {
        using var world = new World();
        var entity = world.Spawn();
        Identity identity = entity;
        Assert.Equal(entity.Id, identity);
    }


    [Fact]
    public void Entity_is_Disposable()
    {
        using var world = new World();
        var builder = world.Spawn();
        Assert.IsAssignableFrom<IDisposable>(builder);
        builder.Dispose();
    }


    [Fact]
    public void Entity_provides_Has()
    {
        using var world = new World();
        var entity = world.Spawn();
        entity.Add(123);

        Assert.True(entity.Has<int>());
        Assert.True(entity.Has<int>(Match.Plain));
        Assert.True(entity.Has<int>(Match.Any));

        Assert.False(entity.Has<int>(Match.Entity));
        Assert.False(entity.Has<int>(Match.Object));
        Assert.False(entity.Has<int>(Match.Target));

        Assert.False(entity.Has<float>(Match.Any));
    }


    [Fact]
    public void Entity_provides_HasLink()
    {
        using var world = new World();
        var entity = world.Spawn();
        world.Spawn();
        entity.AddLink("hello world");

        Assert.True(entity.HasLink<string>("hello world"));
        Assert.True(entity.Has<string>(Match.Any));
        Assert.True(entity.Has<string>(Match.Object));
        Assert.True(entity.Has<string>(Match.Target));

        Assert.False(entity.HasLink<string>("goodbye world"));
        Assert.False(entity.Has<int>(Match.Entity));
    }


    [Fact]
    public void Entity_provides_HasLink_overload_With_Implied_MatchExpression()
    {
        using var world = new World();
        var entity = world.Spawn();
        world.Spawn();
        entity.AddLink("hello world");

        Assert.True(entity.HasLink<string>());
        Assert.False(entity.HasLink<EntityTests>());
    }


    [Fact]
    public void Entity_provides_HasRelation()
    {
        using var world = new World();
        var entity = world.Spawn();
        var target = world.Spawn();
        entity.AddRelation<int>(target);

        Assert.True(entity.HasRelation<int>(target));
        Assert.True(entity.Has<int>(Match.Target));
        Assert.True(entity.Has<int>(Match.Any));

        Assert.False(entity.HasRelation<int>(new Entity(world, new Identity(9001))));
        Assert.False(entity.Has<int>(Match.Object));
    }


    [Fact]
    public void Entity_provides_HasRelation_overload_With_Implied_MatchExpression()
    {
        using var world = new World();
        var entity = world.Spawn();
        var target = world.Spawn();
        entity.AddRelation<int>(target);

        Assert.True(entity.HasRelation<int>());
        Assert.False(entity.HasRelation<float>());
    }


    [Fact]
    public void Can_Get_Component_as_Ref()
    {
        using var world = new World();
        var entity = world.Spawn();
        entity.Add(123);
        ref var component = ref entity.Ref<int>();
        Assert.Equal(123, component);
        component = 456;
        Assert.Equal(456, entity.Ref<int>());
    }


    [Fact]
    public void Can_Get_Link_Object_as_Ref()
    {
        using var world = new World();
        var entity = world.Spawn();
        const string HELLO_WORLD = "hello world";
        entity.AddLink(HELLO_WORLD);
        ref var component = ref entity.Ref<string>(Identity.Of(HELLO_WORLD));
        Assert.Equal("hello world", component);
    }


    [Fact]
    public void Can_Get_Relation_Backing_as_Ref()
    {
        using var world = new World();
        var entity = world.Spawn();
        var target = world.Spawn();
        entity.AddRelation<int>(target);
        ref var component = ref entity.Ref<int>(target);
        Assert.Equal(0, component);
        component = 123;
        Assert.Equal(123, entity.Ref<int>(target));
    }
}