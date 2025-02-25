//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util.math;

internal class LongRandom {
    public readonly long seed;
    private readonly Random rand1;
    private readonly Random rand2;

    public LongRandom() {
        rand1 = Random.Shared;
        rand2 = Random.Shared;
    }

    public LongRandom(long seed) {
        
        this.seed = seed;
        
        int seed1 = 0;
        int seed2 = 0;
        
        for (int i = 0; i < 32; i++) {
            seed1 += (int)(seed & (0b1 << i));
            seed2 += (int)(seed & (0b1 << (i + 1)));
        }
        
        rand1 = new Random(seed1);
        rand2 = new Random(seed2);
    }

    public int NextInt() {
        int result = rand1.Next();
        int mask = rand2.Next();

        result ^= mask;
        
        return result;
    }

    public int NextInt(int min, int max) {
        throw new NotImplementedException();
    }

    public long NextLong() {
        long result = rand1.NextInt64();
        long mask = rand2.NextInt64();

        result ^= mask;
        
        return result;
    }
    
    
    // TODO implement these mfs
    public int NextLong(int min, int max) {
        throw new NotImplementedException();
    }

    public double NextDouble() {
        throw new NotImplementedException();
    }

    public double NextDouble(double min, double max) {
        throw new NotImplementedException();
    }

    public bool NextBool() {
        return (rand1.Next() > 0) ^ (rand2.Next() >= 0);
    }
}