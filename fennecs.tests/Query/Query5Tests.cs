﻿namespace fennecs.tests.Query;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable once ClassNeverInstantiated.Global
public class Query5Tests
{
    [Theory]
    [ClassData(typeof(QueryCountGenerator))]
    private void All_Runners_Applicable(int count, bool createEmptyTable)
    {
        using var world = new World();

        var query = world.Query<TypeA, double, int, string, char>().Build();

        //Create an empty table by spawning and despawning a single entity
        //that matches our test Query (but is a larger Archetype)
        if (createEmptyTable)
        {
            var dead = world.Spawn().Add<int>().Add<TypeA>().Add<char>().Add<double>().Add(0.25f).Add("will be removed");
            world.Despawn(dead);
        }

        for (var index = 0; index < count; index++)
        {
            Assert.Equal(index, query.Count);

            world.Spawn()
                .Add<TypeA>()
                .Add(99.999)
                .Add(index)
                .Add("one")
                .Add('Q');
        }

        query.For(static (ref TypeA _, ref double _, ref int _, ref string str, ref char _) =>
        {
            Assert.Equal("one", str);
            str = "two";
        });

        query.Raw((_, _, integers, strings, _) =>
        {
            for (var i = 0; i < count; i++)
            {
                Assert.Equal(i, integers.Span[i]);
                Assert.Equal("two", strings.Span[i]);
                strings.Span[i] = "three";
            }
        });

        query.Raw((_, _, integers, strings, _) =>
        {
            for (var i = 0; i < count; i++)
            {
                Assert.Equal(i, integers.Span[i]);
                Assert.Equal("three", strings.Span[i]);
                strings.Span[i] = "four";
            }
        });

        query.Job((ref TypeA _, ref double _, ref int index, ref string str, ref char _) =>
        {
            Assert.Equal(index, index);
            Assert.Equal("four", str);
            str = "five";
        }, 4096);

        query.Job(delegate(ref TypeA _, ref double _, ref int index, ref string str, ref char _, int uniform)
        {
            Assert.Equal(index, index);
            Assert.Equal("five", str);
            str = uniform.ToString();
        }, 6, 4096);


        query.For(static (ref TypeA _, ref double _, ref int _, ref string str, ref char _, int uniform) =>
        {
            Assert.Equal(6.ToString(), str);
            str = uniform.ToString();
        }, 7);


        query.Raw((_, _, _, strings, _, uniform) =>
        {
            for (var i = 0; i < count; i++)
            {
                Assert.Equal(7.ToString(), strings.Span[i]);
                strings.Span[i] = uniform.ToString();
            }
        }, 8);

        query.Raw((_, _, _, strings, _, uniform) =>
        {
            for (var i = 0; i < count; i++)
            {
                Assert.Equal(8.ToString(), strings.Span[i]);
                strings.Span[i] = uniform.ToString();
            }
        }, 9);


        query.For((ref TypeA _, ref double _, ref int _, ref string str, ref char _) => { Assert.Equal(9.ToString(), str); });
    }


    private struct TypeA;
}