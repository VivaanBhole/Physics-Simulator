using System;
using UnityEngine;

public class ConfigValue
{
    [Flags]
    public enum Type
    {
        Checkbox = 1,
        NumField = 2
    }

    public readonly Type type;
    private float[] valueRange = new float[] {-1, 0, -1};
    public float Value
    {
        get
        {
            return valueRange[1];
        }
        set
        {
            bool inBounds = valueRange[0] <= value && value <= valueRange[2];

            if (inBounds)
            {
                valueRange[1] = value;
                return;
            }

            if (value <= valueRange[0])
                valueRange[1] = valueRange[0];
            else if (value >= valueRange[2])
                valueRange[1] = valueRange[2];
        }
    }
    public bool Checkbox { get; set; } = false;

    public ConfigValue(Type flags, bool checkboxInitialState = false, float[] valueRange = null)
    {
        type = flags;
        Checkbox = checkboxInitialState;
        if (valueRange != null) 
        { 
            this.valueRange = valueRange; 
        }
    }

}
