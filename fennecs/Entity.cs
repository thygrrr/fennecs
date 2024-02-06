// SPDX-License-Identifier: MIT

namespace fennecs;

public readonly struct Entity(Identity identity)
{
    public static readonly Entity None = default;
    public static readonly Entity Any = new(Identity.Any);

    internal Identity Identity { get; } = identity;

    public bool IsType => Identity.IsType();

    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Identity.Equals(entity.Identity);
    }

    public Type Type => Identity.Type;

    public override int GetHashCode()
    {
        return Identity.GetHashCode();
    }

    
    public override string ToString()
    {
        return Identity.ToString();
    }

    public static implicit operator Identity(Entity left) => left.Identity;
    
    public static bool operator ==(Entity left, Entity right) => left.Equals(right);

    public static bool operator !=(Entity left, Entity right) => !left.Equals(right);

    public static bool operator ==(Identity left, Entity right) => left.Equals(right.Identity);

    public static bool operator !=(Identity left, Entity right) => !left.Equals(right);

    public static bool operator ==(Entity left, Identity right) => left.Identity.Equals(right);

    public static bool operator !=(Entity left, Identity right) => !left.Identity.Equals(right);
}

public readonly struct EntityBuilder(World world, Entity entity)
{
    public EntityBuilder Add<T>(Entity target = default) where T : struct
    {
        if (target.Identity == Identity.Any) throw new InvalidOperationException("EntityBuilder: Cannot relate to Identity.Any.");
        world.AddComponent<T>(entity, target);
        return this;
    }

    public EntityBuilder Add<T>(Type type) where T : struct
    {
        var typeEntity = world.GetTypeEntity(type);
        world.AddComponent<T>(entity, typeEntity);
        return this;
    }

    
    public EntityBuilder Add<T>(T data)
    {
        world.AddComponent(entity, data);
        return this;
    }

    
    public EntityBuilder Add<T>(Entity target, T data) where T : struct
    {
        if (target.Identity == Identity.Any) throw new InvalidOperationException("EntityBuilder: Cannot relate to Identity.Any.");
        
        world.AddComponent(entity, data, target);
        return this;
    }

    
    public EntityBuilder Add<T>(T data, Type type) where T : struct
    {
        var typeEntity = world.GetTypeEntity(type);
        world.AddComponent(entity, data, typeEntity);
        return this;
    }

    
    public EntityBuilder Remove<T>() where T : struct
    {
        world.RemoveComponent<T>(entity);
        return this;
    }

    
    public EntityBuilder Remove<T>(Entity target) where T : struct
    {
        world.RemoveComponent<T>(entity, target);
        return this;
    }

    
    public EntityBuilder Remove<T>(Type type) where T : struct
    {
        var typeEntity = world.GetTypeEntity(type);
        world.RemoveComponent<T>(entity, typeEntity);
        return this;
    }

    public Entity Id()
    {
        return entity;
    }
}