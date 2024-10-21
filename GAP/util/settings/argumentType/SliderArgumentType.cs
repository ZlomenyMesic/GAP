//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Slider Argument Type
/// </summary>
public class SliderArgumentType : ArgumentType {
    
    public int nmin { get; }
    public int nmax { get; }
    public uint nstep { get; }
    private int? nvalue { get; set; }
    
    public double dmin { get; }
    public double dmax { get; }
    public double dstep { get; }
    private double? dvalue { get; set; }
    
    public bool isNatural { get; }

    public SliderArgumentType(int min = Int32.MinValue, int max = Int32.MaxValue, uint step = 1) {
        if (step == 0)
            throw new SettingsBuilderException("Step cannot be less than 1.");
        
        nmin = min;
        nmax = max;
        nstep = step;
        isNatural = true;
    }

    public SliderArgumentType(double min = Double.MinValue, double max = Double.MaxValue, double step = 1) {
        if (step <= 0)
            throw new SettingsBuilderException("Step cannot be less than 1.");
        
        dmin = min;
        dmax = max;
        dstep = step;
        isNatural = false;
    }
    
    public string GetInputType() {
        return "Slider";
    }

    public string GetValue() {
        if (isNatural) {
            if (nvalue == null) throw new SettingsBuilderException("Accessing value that has not been set.");
            return nvalue.ToString() ?? string.Empty;
        }
        else {
            if (dvalue == null) throw new SettingsBuilderException("Accessing value that has not been set.");
            return dvalue.ToString() ?? string.Empty;
        }
    }

    public object GetParsedValue() {
        if (isNatural) {
            if (nvalue == null) throw new SettingsBuilderException("Accessing value that has not been set.");
            return nvalue;
        }
        else {
            if (dvalue == null) throw new SettingsBuilderException("Accessing value that has not been set.");
            return dvalue;
        }
    }

    public void SetValue(string value) {
        if (isNatural) {
            nvalue = Arguments.ParseNaturalSlider(value, this);
        }
        else {
            dvalue = Arguments.ParseDecimalSlider(value, this);
        }
    }

    public void SetParsedValue(object value) {
        if (isNatural) {
            if (value.GetType() != typeof(int))
                throw new SettingsArgumentException("Invalid value type.");
            
            nvalue = Arguments.ParseNaturalSlider((int)value, this);
        }
        else {
            if (value.GetType() != typeof(double))
                throw new SettingsArgumentException("Invalid value type.");
            
            dvalue = Arguments.ParseDecimalSlider((double)value, this);
        }
    }

    public ArgumentType Clone() {
        return (ArgumentType)MemberwiseClone();
    }

    public override string ToString() {
        return $"{{\"type\": \"slider\", " +
               $"\"isNatural\": {isNatural}," +
               $"\"nmin\": {nmin}," +
               $"\"nmax\": {nmax}," +
               $"\"nstep\": {nstep}," +
               $"\"nvalue\": {(nvalue == null ? "\"null\"" : nvalue)}," +
               $"\"dmin\": {dmin}," +
               $"\"dmax\": {dmax}," +
               $"\"dstep\": {dstep}," +
               $"\"dvalue\": {(dvalue == null ? "\"null\"" : dvalue)}}}";
    }
}