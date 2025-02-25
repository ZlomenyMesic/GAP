//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util;

public static class SeedFormat {

    /// <summary>
    /// transforms a numWord string into an integer
    /// </summary>
    /// <param name="seedWord">numWord string</param>
    /// <returns>transformed integer</returns>
    public static int SeedFromWord(string seedWord) {

        List<byte> digits = new List<byte>();
        
        for (int i = seedWord.Length / 2 - 1; i >= 0; i--) {
            digits.Add(FromSyllable(seedWord[i * 2], seedWord[i * 2 + 1]));
        }
        
        return BaseConverter.ToDecimal(digits.ToArray(), 120);
    }

    
    /// <summary>
    /// transforms an integer into a numWord
    /// </summary>
    /// <param name="seed">integer</param>
    /// <returns>transformed numWord string</returns>
    public static string WordFromSeed(int seed) {
        
        byte[] digits = BaseConverter.FromDecimal(seed, 120);
        string[] syllables = new string[digits.Length];

        for (int i = 0; i < digits.Length; i++) {
            syllables[i] = ToSyllable(digits[i]);
        }
        
        string word = "";

        foreach (var s in syllables) {
            word += s;
        }
        
        return word;
    }

    
    private static readonly byte[] CHAR_VALUE = 
        [ 
            000, 000, 001, 002, 020, 003, 004, 005, 040, 006, 007, 008, 009,
            010, 060, 011, 012, 013, 014, 015, 080, 016, 017, 018, 100, 019 
        ];

    private const string VOWELS = "aeiouy";
    private const string CONSONANTS = "bcdfghjklmnpqrstvwxz";

    private static byte FromSyllable(char consonant, char vowel) => (byte)(CharToByte(consonant) + CharToByte(vowel));
    private static string ToSyllable(byte value) => "" + ToConsonant(value) + ToVowel(value);
    private static byte CharToByte(char c) => CHAR_VALUE[(byte)char.ToLower(c) - 0x61];
    private static char ToVowel(byte value) => VOWELS[value / 20];
    private static char ToConsonant(byte value) => CONSONANTS[value % 20];
}