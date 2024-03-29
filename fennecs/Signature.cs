﻿// SPDX-License-Identifier: MIT

using System.Collections;
using System.Collections.Immutable;

namespace fennecs;

/// <summary>
/// Generic IImmutableSortedSet whose hash code is a combination of its elements' hashes.
/// </summary>
public readonly struct Signature<T> : IEquatable<Signature<T>>, IEnumerable<T>
    where T : notnull
{
    private readonly ImmutableSortedSet<T> _set = ImmutableSortedSet<T>.Empty;
    private readonly int _hashCode;

    public override int GetHashCode() => _hashCode;


    public Signature(params T[] values) : this(values.ToImmutableSortedSet())
    {
    }


    public Signature(ImmutableSortedSet<T> set)
    {
        _set = set;
        Count = set.Count;
        _hashCode = BakeHash(_set);
    }


    public Signature<T> Add(T value) => new(_set.Add(value));


    public Signature<T> Clear() => new(ImmutableSortedSet<T>.Empty);


    public bool Contains(T item) => _set.Contains(item);


    public Signature<T> Except(IEnumerable<T> other) => new(_set.Except(other));


    public Signature<T> Intersect(IEnumerable<T> other) => new(_set.Intersect(other));


    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);


    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);


    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);


    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);


    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);


    public Signature<T> Remove(T value) => new(_set.Remove(value));


    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);


    public Signature<T> SymmetricExcept(IEnumerable<T> other) => new(_set.SymmetricExcept(other));


    public bool TryGetValue(T equalValue, out T actualValue) => _set.TryGetValue(equalValue, out actualValue);


    public Signature<T> Union(IEnumerable<T> other) => new(_set.Union(other));


    public bool Equals(Signature<T> other) => _set.SetEquals(other._set);


    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();


    public override bool Equals(object? obj) => obj is Signature<T> other && Equals(other);


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    private static int BakeHash(IEnumerable<T> source)
    {
        var code = new HashCode();
        foreach (var item in source) code.Add(item);
        return code.ToHashCode();
    }


    public static bool operator ==(Signature<T> left, Signature<T> right) => left.Equals(right);

    public static bool operator !=(Signature<T> left, Signature<T> right) => !left.Equals(right);


    public int Count { get; }

    public T this[int index] => _set.ElementAt(index);
}