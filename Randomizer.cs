using System;
using System.Collections.Generic;
using Interfaces;

public static class Randomizer<T> where T : IChance
{
    private static Random _rand = new Random();

    public static T Randomize(List<T> needToRandomize)
    {
        T value = default(T);

        int chanceSum = 0;

        foreach (var item in needToRandomize)
        {
            chanceSum += item.Chance;
        }

        int randomChance = _rand.Next(0, chanceSum);
        chanceSum = 0;
        foreach (var item in needToRandomize)
        {
            chanceSum += item.Chance;
            if (randomChance < chanceSum)
            {
                value = item;
                break;
            }
        }
        
        return value;
    }

    public static T Randomize(List<T> needToRandomize, int appearChance)
    {
        int randomAppearChance = _rand.Next(0, 100);

        if (appearChance >= randomAppearChance)
        {
            return Randomize(needToRandomize);
        }
        return default(T);
    }
}