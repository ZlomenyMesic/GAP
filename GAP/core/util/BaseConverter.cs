//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util;

public static class BaseConverter {
    
    /// <summary>
    /// converts series of digits of a base (max 128) to another series of digits of another base (max 128)
    /// </summary>
    /// <param name="input">input series of digits</param>
    /// <param name="inputBase">input base</param>
    /// <param name="outputBase">output base</param>
    /// <returns>series of outputBase base digits</returns>
    /// <exception cref="ArgumentException">if an inputted base is less than 1 or more than 128</exception>
    public static byte[] Convert(byte[] input, int inputBase, int outputBase) {
        
        if (inputBase is <= 0 or > 128) 
            throw new ArgumentException($"Base must be positive and lower than 128! Instead it is {inputBase}");
        
        if (outputBase is <= 0 or > 128) 
            throw new ArgumentException($"Base must be positive and lower than 128! Instead it is {outputBase}");


        int val = ToDecimal(input, inputBase);

        return FromDecimal(val, outputBase);
    }
    
    
    /// <summary>
    /// converts a series of digits into an integer
    /// </summary>
    /// <param name="input">series of digits</param>
    /// <param name="inputBase">input base</param>
    /// <returns>an integer value</returns>
    public static int ToDecimal(byte[] input, int inputBase) {
        int output = 0;

        for (int i = 0; i < input.Length; i++) {
            output += input[i] * (int)Math.Pow(inputBase, i);
        }
        
        return output;
    }
    
    
    /// <summary>
    /// converts an integer into a series of digits in outputBase base
    /// </summary>
    /// <param name="input">an integer value</param>
    /// <param name="outputBase">output base</param>
    /// <returns>series of digits in outputBase base</returns>
    public static byte[] FromDecimal(int input, int outputBase) {
        
        if (input == 0) return [0];

        var result = new Stack<byte>();
        while (input > 0)
        {
            result.Push((byte)(input % outputBase));
            input /= outputBase;
        }

        return result.ToArray();
    }
}
