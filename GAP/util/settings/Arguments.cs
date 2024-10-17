//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using GAP.util.settings.argumentType;

namespace GAP.util.settings;

/// <summary>
/// easy access to ArgumentType classes
/// </summary>
public static class Arguments {

    /// <summary>
    /// universal parsing method
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argumentType">argument type</param>
    /// <returns>parsed value</returns>
    /// <exception cref="ArgumentException">unknown argument type inputted</exception>
    /// <exception cref="SettingsArgumentException">argument failed to parse</exception>
    public static object Parse(string value, ArgumentType argumentType) {
        return argumentType switch {
            IntegerArgumentType type => ParseInteger(value, type),
            UnsignedIntegerArgumentType type => ParseUInteger(value, type),
            LongArgumentType type => ParseLong(value, type),
            UnsignedLongArgumentType type => ParseULong(value, type),
            FloatArgumentType type => ParseFloat(value, type),
            DoubleArgumentType type => ParseDouble(value, type),
            StringArgumentType type => ParseString(value, type),
            BoolArgumentType => ParseBool(value),
            _ => throw new ArgumentException($"Unknown argument type: {argumentType.GetType().Name}")
        };
    }

    /// <summary>
    /// copies value of an argument to another argument
    /// </summary>
    /// <param name="from">source argument</param>
    /// <param name="to">target argument</param>
    /// <exception cref="SettingsArgumentException">argument types are not matching</exception>
    public static void CopyValue(ref ArgumentType from, ref ArgumentType to) {
        if (from.GetType() != to.GetType()) {
            throw new SettingsArgumentException(from, to);
        }
        
        to.SetValue(to.GetValue());
    }
    
    
    
    // --- integer things ---
    
    /// <summary>
    /// returns a new integer argument
    /// </summary>
    public static IntegerArgumentType Integer(int min = Int32.MinValue, int max = Int32.MaxValue) => new(min, max);

    /// <summary>
    /// parses a string using an integer argument
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argument">integer argument</param>
    /// <returns>parsed integer value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    /// <exception cref="SettingsArgumentException">number is out of bounds</exception>
    public static int ParseInteger(string value, IntegerArgumentType argument) {
        int output;
        
        try {
            output = int.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }

        if (output < argument.min || output > argument.max) {
            throw new SettingsArgumentException(argument, 
                $"Invalid value: {value}, must be between {argument.min} and {argument.max}.");
        }
        
        return output;
    }
    
    
    
    // --- unsigned integer things ---
    
    /// <summary>
    /// returns a new unsigned integer argument
    /// </summary>
    public static UnsignedIntegerArgumentType UInteger(uint min = UInt32.MinValue, uint max = UInt32.MaxValue) => 
        new(min, max);

    /// <summary>
    /// parses a string using an unsigned integer argument
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argument">unsigned integer argument</param>
    /// <returns>parsed unsigned integer value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    /// <exception cref="SettingsArgumentException">number is out of bounds</exception>
    public static uint ParseUInteger(string value, UnsignedIntegerArgumentType argument) {
        uint output;
        
        try {
            output = uint.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }

        if (output < argument.min || output > argument.max) {
            throw new SettingsArgumentException(argument, 
                $"Invalid value: {value}, must be between {argument.min} and {argument.max}.");
        }
        
        return output;
    } 
    
    
    
    // --- long things ---
    
    /// <summary>
    /// returns a new long argument
    /// </summary>
    public static LongArgumentType Long(long min = Int64.MinValue, long max = Int64.MaxValue) => new(min, max);

    /// <summary>
    /// parses a string using a long argument
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argument">long argument</param>
    /// <returns>parsed long value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    /// <exception cref="SettingsArgumentException">number is out of bounds</exception>
    public static long ParseLong(string value, LongArgumentType argument) {
        long output;
        
        try {
            output = long.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }

        if (output < argument.min || output > argument.max) {
            throw new SettingsArgumentException(argument, 
                $"Invalid value: {value}, must be between {argument.min} and {argument.max}.");
        }
        
        return output;
    }
    
    
    
    // --- unsigned long things ---
    
    /// <summary>
    /// returns a new unsigned long argument
    /// </summary>
    public static UnsignedLongArgumentType ULong(ulong min = UInt64.MinValue, ulong max = UInt64.MaxValue) 
        => new(min, max);

    /// <summary>
    /// parses a string using an unsigned long argument
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argument">unsigned long argument</param>
    /// <returns>parsed unsigned long value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    /// <exception cref="SettingsArgumentException">number is out of bounds</exception>
    public static ulong ParseULong(string value, UnsignedLongArgumentType argument) {
        ulong output;
        
        try {
            output = ulong.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }

        if (output < argument.min || output > argument.max) {
            throw new SettingsArgumentException(argument, 
                $"Invalid value: {value}, must be between {argument.min} and {argument.max}.");
        }
        
        return output;
    } 
    
    
    
    // --- float things ---
    
    /// <summary>
    /// returns a new float argument
    /// </summary>
    public static FloatArgumentType Float(float min = Single.MinValue, float max = Single.MaxValue) => new(min, max);
    
    /// <summary>
    /// parses a string using a float argument
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argument">float argument</param>
    /// <returns>parsed float value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    /// <exception cref="SettingsArgumentException">number is out of bounds</exception>
    public static float ParseFloat(string value, FloatArgumentType argument) {
        float output;
        
        try {
            output = float.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }

        if (output < argument.min || output > argument.max) {
            throw new SettingsArgumentException(argument, 
                $"Invalid value: {value}, must be between {argument.min} and {argument.max}.");
        }
        
        return output;
    }
    
    
    
    // --- double things ---
    
    /// <summary>
    /// returns a new double argument
    /// </summary>
    public static DoubleArgumentType Double(double min = System.Double.MinValue, double max = System.Double.MaxValue) => 
        new(min, max);
    
    /// <summary>
    /// parses a string using a double argument
    /// </summary>
    /// <param name="value">string value</param>
    /// <param name="argument">double argument</param>
    /// <returns>parsed double value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    /// <exception cref="SettingsArgumentException">number is out of bounds</exception>
    public static double ParseDouble(string value, DoubleArgumentType argument) {
        double output;
        
        try {
            output = double.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }

        if (output < argument.min || output > argument.max) {
            throw new SettingsArgumentException(argument, 
                $"Invalid value: {value}, must be between {argument.min} and {argument.max}.");
        }
        
        return output;
    }
    
    
    
    // --- bool things ---
    
    /// <summary>
    /// returns a new bool argument
    /// </summary>
    public static BoolArgumentType Bool() => new();

    /// <summary>
    /// parses a string to a bool
    /// </summary>
    /// <param name="value">string value</param>
    /// <returns>parsed bool value</returns>
    /// <exception cref="FormatException">could not parse the value</exception>
    public static bool ParseBool(string value) {
        
        try {
            return bool.Parse(value);
        }
        catch (FormatException e) {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    
    // --- string things ---
    
    /// <summary>
    /// returns a new string argument
    /// </summary>
    public static StringArgumentType String(uint minLength = 0, uint maxLength = UInt32.MaxValue) => 
        new(minLength, maxLength);

    /// <summary>
    /// parses a string
    /// </summary>
    /// <param name="value">input string</param>
    /// <param name="argument">string argument</param>
    /// <returns>parsed string argument</returns>
    /// <exception cref="SettingsArgumentException">string is too long</exception>
    public static string ParseString(string value, StringArgumentType argument) {
        
        if (value.Length < argument.minLength) {
            throw new SettingsArgumentException(argument, 
                $"The value is too short, min length is {argument.minLength}.");
        }
        
        if (value.Length > argument.maxLength) {
            throw new SettingsArgumentException(argument, 
                $"The value is too long, max length is {argument.maxLength}.");
        }
        
        return value;
    }
    
    
    
    // TODO finish multi select and list argument parsers
    
    // --- multi select things ---
    
    /// <summary>
    /// returns a new multi select argument
    /// </summary>
    public static MultiSelectArgumentType MultiSelect(Type type) => new(type);
    
    /// <summary>
    /// returns a new multi select argument
    /// </summary>
    public static MultiSelectArgumentType MultiSelect<T>() => new(typeof(T));
    
    /// <summary>
    /// returns a new multi select argument
    /// </summary>
    public static MultiSelectArgumentType MultiSelect(string[] values) => new(values);
    
    
    
    // --- single select things ---
    
    /// <summary>
    /// returns a new single select argument
    /// </summary>
    public static SingleSelectArgumentType SingleSelect(Type type) => new(type);
    
    /// <summary>
    /// returns a new single select argument
    /// </summary>
    public static SingleSelectArgumentType SingleSelect<T>() => new(typeof(T));
    
    /// <summary>
    /// returns a new single select argument
    /// </summary>
    public static SingleSelectArgumentType SingleSelect(string[] values) => new(values);

    
    
    // --- select list things ---
    
    /// <summary>
    /// returns a new select list argument
    /// </summary>
    public static SelectListArgumentType SelectList(Type type, int maxItemCount = Int32.MaxValue) => 
        new(type, maxItemCount);
    
    /// <summary>
    /// returns a new select list argument
    /// </summary>
    public static SelectListArgumentType SelectList<T>(int maxItemCount = Int32.MaxValue) => 
        new(typeof(T), maxItemCount);
    
    /// <summary>
    /// returns a new select list argument
    /// </summary>
    public static SelectListArgumentType SelectList(string[] values, int maxItemCount = Int32.MaxValue) =>
        new(values, maxItemCount);
    
    
    
    // --- free list things ---
    
    /// <summary>
    /// returns a new free list argument
    /// </summary>
    public static FreeListArgumentType FreeList(ArgumentType type, int maxItemCount = Int32.MaxValue) => 
        new(type, maxItemCount);
}